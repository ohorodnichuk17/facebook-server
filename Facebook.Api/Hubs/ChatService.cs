using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Chat;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static Facebook.Domain.Common.Errors.Errors;

namespace Facebook.Server.Hubs;

public class ChatService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IUnitOfWork _unitOfWork;
    public ChatService(IHubContext<ChatHub> hubContext, IUnitOfWork unitOfWork)
    {
        _hubContext = hubContext;
        _unitOfWork = unitOfWork;
    }
    //public async Task SendMessage(string fromUserEmail, string toUserEmail, string messageContent)
    //{
    //    var fromUser = await _unitOfWork.User.GetByEmailAsync(fromUserEmail);
    //    var toUser = await _unitOfWork.User.GetByEmailAsync(toUserEmail);

    //    if (fromUser.IsError || toUser.IsError)
    //    {
    //        return;
    //    }

    //    var chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value.Id, toUser.Value.Id);
    //    if (chat.IsError)
    //    {
    //        chat = new ChatEntity
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = toUser.Value?.Email ?? "Unknown",
    //            ChatUsers = new List<ChatUserEntity>
    //            {
    //                new ChatUserEntity { UserId = fromUser.Value.Id },
    //                new ChatUserEntity { UserId = toUser.Value?.Id ?? Guid.Empty }
    //            }
    //        };
    //        await _unitOfWork.Chat.CreateAsync(chat.Value);
    //        chat = await _unitOfWork.Chat.GetChatByUsersIdAsync(fromUser.Value?.Id
    //                                                            ?? Guid.Empty, toUser.Value?.Id ?? Guid.Empty);
    //    }

    //    var message = new MessageEntity
    //    {
    //        Id = Guid.NewGuid(),
    //        Content = messageContent,
    //        UserId = fromUser.Value?.Id ?? Guid.Empty,
    //        ChatId = chat.Value?.Id ?? Guid.Empty,
    //        CreatedAt = DateTime.UtcNow
    //    };
    //    await _unitOfWork.Message.CreateAsync(message);

    //    //await _hubContext.Clients.Group(chat.Value?.Id.ToString() ?? string.Empty).SendAsync("ReceiveMessage", fromUserEmail, messageContent);
    //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", fromUserEmail, messageContent);
    //}

    public async Task SendMessage(string userName, string messageContent)
    {
        var user = await _unitOfWork.User.GetByEmailAsync(userName);

        var message = new MessageEntity
        {
            Content = messageContent,
            CreatedAt = DateTime.Now,
            UserId = user.Value.Id
        };

        await _unitOfWork.Message.CreateAsync(message);

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}
