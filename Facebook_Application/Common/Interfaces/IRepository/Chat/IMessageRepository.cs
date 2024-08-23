using ErrorOr;
using Facebook.Domain.Chat;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository.Chat;

public interface IMessageRepository : IRepository<MessageEntity>
{
    Task<ErrorOr<IEnumerable<MessageEntity>>> GetMessagesByChatIdAsync(Guid chatId);
    Task<ErrorOr<Unit>> UpdateMessageAsync(MessageEntity message);
}
