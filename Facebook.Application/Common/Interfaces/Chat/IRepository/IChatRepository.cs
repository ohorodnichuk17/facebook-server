using Facebook.Domain.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Common.Interfaces.Chat.IRepository;

public interface IChatRepository
{
    Task<IEnumerable<ChatEntity>> GetByUserIdAsync(Guid userId);
}
