using Facebook.Application.Common.Interfaces.Chat.IRepository;
using Microsoft.AspNetCore.SignalR;
using Facebook.Domain.Chat;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using ErrorOr;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return; // Log error or notify client
        }

        var chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value.Id, toUser.Value.Id);
        if (chat.IsError)
        {
            chat = new ChatEntity
            {
                Id = Guid.NewGuid(),
                Name = toUser.Value.Email,
                ChatUsers = new List<ChatUserEntity>
                {
                    new ChatUserEntity { UserId = fromUser.Value.Id },
                    new ChatUserEntity { UserId = toUser.Value.Id }
                }
            };
            await _unitOfWork.Chat.CreateAsync(chat.Value);
            chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value.Id, toUser.Value.Id);
        }

        var message = new MessageEntity
        {
            Id = Guid.NewGuid(),
            Content = messageContent,
            UserId = fromUser.Value.Id,
            ChatId = chat.Value.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Message.CreateAsync(message);

        await Clients.Group(chat.Value.Id.ToString()).SendAsync("ReceiveMessage", fromUserEmail, messageContent);
    }

    public override async Task OnConnectedAsync()
    {
        var email = Context.GetHttpContext().Request.Query["email"];
        var user = await _userRepository.GetByEmailAsync(email.ToString());

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

        await base.OnConnectedAsync();
    }

    //public async Task<IEnumerable<MessageEntity>> GetMessages(string fromUserEmail, string toUserEmail)
    //{
    //    var fromUser = await _userRepository.GetByEmailAsync(fromUserEmail);
    //    var toUser = await _userRepository.GetByEmailAsync(toUserEmail);

    //    if (fromUser.IsError || toUser.IsError)
    //    {
    //        return new List<MessageEntity>(); // Return an empty list if either user is not found
    //    }

    //    var chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value.Id, toUser.Value.Id);
    //    if (chat.IsError || chat.Value == null)
    //    {
    //        return new List<MessageEntity>(); // Return an empty list if the chat is not found
    //    }

    //    var messages = await _unitOfWork.Message.GetMessagesByChatIdAsync(chat.Value.Id);
    //    return messages.Value;
    //}
}
