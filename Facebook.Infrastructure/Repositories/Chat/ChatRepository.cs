using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Chat;
using Facebook.Domain.Chat;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Chat;

public class ChatRepository(FacebookDbContext context) : Repository<ChatEntity>(context), IChatRepository
{
    public async Task<ErrorOr<ChatEntity>> GetChatByUsersIdAsync(Guid senderId, Guid receiverId)
    {
        var chat = await context.Chats
        .Include(c => c.ChatUsers)
        .Include(c => c.Users)
        .SingleOrDefaultAsync(c =>
            c.ChatUsers.Any(u => u.UserId == senderId) &&
            c.ChatUsers.Any(u => u.UserId == receiverId));

        return chat == null ? Error.NotFound() : chat;
    }

    public async Task<ErrorOr<IEnumerable<ChatEntity>>> GetChatsByUserIdAsync(Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            return Error.NotFound();
        }

        var chats = await context.Chats
            .Include(c => c.Messages)
            .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
            .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId))
            .ToListAsync();

        chats.ForEach(c => c.Messages = c.Messages.OrderBy(m => m.CreatedAt).ToList());

        return chats;
    }
}
