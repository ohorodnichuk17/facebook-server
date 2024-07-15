using ErrorOr;
using Facebook.Application.Common.Interfaces.Chat.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Chat;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Infrastructure.Repositories.User;
using LanguageExt;
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
    public async Task<ErrorOr<ChatEntity>> GetChatByUsersIdAsync(Guid senderId, Guid receiverId)
    {
        return await context.Chats
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Users.Any(u => u.Id == senderId) && c.Users.Any(u => u.Id == receiverId));
    }

    public async Task<ErrorOr<IEnumerable<ChatEntity>>> GetByUserIdAsync(Guid userId)
    {
        return await context.Chats
            .Include(c => c.Users)
            .Where(c => c.Users.Any(uc => uc.Id == userId))
            .ToListAsync();
    }
}
