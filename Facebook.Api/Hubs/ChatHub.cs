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

        if (toUser.IsError)
        {
            return;
        }

        var chatResult = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value.Id, toUser.Value.Id);
        ChatEntity chat;
        if (chatResult.IsError)
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
            await _unitOfWork.Chat.CreateAsync(chat);

            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
        }
        else
        {
            chat = chatResult.Value;
        }

        var message = new MessageEntity
        {
            Id = Guid.NewGuid(),
            Content = messageContent,
            UserId = fromUser.Value.Id,
            ChatId = chat.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Message.CreateAsync(message);

        var chatUsers = chat.ChatUsers;
        foreach (var chatUser in chatUsers)
        {
            var user = await _userRepository.GetUserByIdAsync(chatUser.UserId);
            var connectionId = GetUserConnectionId(user.Value.Email);
            await Groups.AddToGroupAsync(connectionId, chat.Id.ToString());
        }

        await Clients.Group(chat.Id.ToString()).SendAsync("ReceiveMessage", fromUserEmail, messageContent);
    }

    private string GetUserConnectionId(string email)
    {
        return string.Empty;
    }
}
