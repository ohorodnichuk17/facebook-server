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
        var chat = await context.Chats
        .Include(c => c.ChatUsers)
        .SingleOrDefaultAsync(c =>
            c.ChatUsers.Any(u => u.UserId == senderId) &&
            c.ChatUsers.Any(u => u.UserId == receiverId));

        return chat == null ? Error.NotFound() : chat;
    }

    public async Task<ErrorOr<IEnumerable<ChatEntity>>> GetByUserIdAsync(Guid userId)
    {
        return await context.Chats
            .Include(c => c.Users)
            .Where(c => c.Users.Any(uc => uc.Id == userId))
            .ToListAsync();
    }
}
