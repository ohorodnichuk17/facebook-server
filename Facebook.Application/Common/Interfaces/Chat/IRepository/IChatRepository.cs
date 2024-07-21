using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository;
using Facebook.Domain.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Common.Interfaces.Chat.IRepository;

public interface IChatRepository : IRepository<ChatEntity>
{
    Task<ErrorOr<IEnumerable<ChatEntity>>> GetChatsByUserIdAsync(Guid userId);
    Task<ErrorOr<ChatEntity>> GetChatByUsersIdAsync(Guid senderId, Guid receiverId);
}
