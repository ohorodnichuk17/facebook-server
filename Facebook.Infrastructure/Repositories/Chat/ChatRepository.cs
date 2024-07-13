using Facebook.Application.Common.Interfaces.Chat.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Chat;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Infrastructure.Repositories.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Facebook.Domain.Common.Errors.Errors;

namespace Facebook.Infrastructure.Repositories.Chat;

public class ChatRepository(FacebookDbContext context) : Repository<ChatEntity>(context), IChatRepository
{
    public async Task<IEnumerable<ChatEntity>> GetByUserIdAsync(Guid userId)
    {
        return await context.Chats
            .Include(c => c.UserChats)
            .Where(c => c.UserChats.Any(uc => uc.UserId == userId))
            .ToListAsync();
    }
}
