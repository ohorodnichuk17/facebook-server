using ErrorOr;
using Facebook.Domain.Chat;

namespace Facebook.Application.Common.Interfaces.IRepository.Chat;

public interface IChatRepository : IRepository<ChatEntity>
{
    Task<ErrorOr<IEnumerable<ChatEntity>>> GetChatsByUserIdAsync(Guid userId);
    Task<ErrorOr<ChatEntity>> GetChatByUsersIdAsync(Guid senderId, Guid receiverId);
}
