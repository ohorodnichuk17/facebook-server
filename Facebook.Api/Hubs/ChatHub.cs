using Facebook.Application.Common.Interfaces.Chat.IRepository;
using Microsoft.AspNetCore.SignalR;
using Facebook.Domain.Chat;
using LanguageExt.Pipes;
using System.Text.RegularExpressions;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using Facebook.Infrastructure.Repositories.Chat;
using Facebook.Application.Common.Interfaces.User.IRepository;
using ErrorOr;
using LanguageExt;

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

        if (toUser.IsError)
        {
            return;
        }

        var chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value.Id, toUser.Value.Id);
        if (chat.Value == null)
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

        await Clients.All.SendAsync("ReceiveMessage", fromUserEmail, messageContent);
    }

    public async Task JoinChat(Guid chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(Guid chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}
