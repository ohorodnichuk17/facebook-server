using Microsoft.AspNetCore.SignalR;
using Facebook.Domain.Chat;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using ErrorOr;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Application.Common.Interfaces.IRepository.User;

namespace Facebook.Server.Hubs;

public class ChatHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public ChatHub(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task SendMessage(string fromUserEmail, string toUserEmail, string messageContent)
    {
        var fromUser = await _userRepository.GetByEmailAsync(fromUserEmail);
        var toUser = await _userRepository.GetByEmailAsync(toUserEmail);

        if (fromUser.IsError || toUser.IsError)
        {
            return; 
        }

        var chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value.Id, toUser.Value.Id);
        if (chat.IsError)
        {
            chat = new ChatEntity
            {
                Id = Guid.NewGuid(),
                Name = toUser.Value?.Email ?? "Unknown",
                ChatUsers = new List<ChatUserEntity>
                {
                    new ChatUserEntity { UserId = fromUser.Value.Id },
                    new ChatUserEntity { UserId = toUser.Value?.Id ?? Guid.Empty }
                }
            };
            await _unitOfWork.Chat.CreateAsync(chat.Value);
            chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value?.Id 
                                                                ?? Guid.Empty, toUser.Value?.Id ?? Guid.Empty);
        }

        var message = new MessageEntity
        {
            Id = Guid.NewGuid(),
            Content = messageContent,
            UserId = fromUser.Value?.Id ?? Guid.Empty,
            ChatId = chat.Value?.Id ?? Guid.Empty,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Message.CreateAsync(message);

        await Clients.Group(chat.Value?.Id.ToString() ?? string.Empty).SendAsync("ReceiveMessage", fromUserEmail, messageContent);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext != null)
        {
            var email = httpContext.Request.Query["email"].ToString();
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _userRepository.GetByEmailAsync(email);

                if (!user.IsError)
                {
                    var chatsResult = await _unitOfWork.Chat.GetChatsByUserIdAsync(user.Value.Id);
                    if (!chatsResult.IsError)
                    {
                        foreach (var chat in chatsResult.Value)
                        {
                            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());

                            var messages = await _unitOfWork.Message.GetMessagesByChatIdAsync(chat.Id);
                            await Clients.Caller.SendAsync("LoadMessages", chat.Id.ToString(), messages);
                        }
                    }
                }
            }
        }

        await base.OnConnectedAsync();
    }

}
