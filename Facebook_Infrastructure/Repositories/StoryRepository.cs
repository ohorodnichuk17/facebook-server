using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Story;
using Facebook.Domain.Story;
using Facebook.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories;

public class StoryRepository(FacebookDbContext context) : Repository<StoryEntity>(context), IStoryRepository
{
    public new async Task<ErrorOr<IEnumerable<StoryEntity>>> GetAllAsync()
    {
        try
        {
            var stories = await dbSet
                .Include(p => p.User)
                .ToListAsync();

            return stories;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<StoryEntity>>> GetFriendsStoriesAsync(Guid userId)
    {
        try
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null) return Error.NotFound();

            var friendIds = await context.FriendRequests
                .Where(uf => (uf.ReceiverId == user.Id || uf.SenderId == user.Id) && uf.IsAccepted)
                .Select(uf => uf.ReceiverId == user.Id ? uf.SenderId : uf.ReceiverId)
                .ToListAsync();

            var stories = await context.Stories
            .Include(p => p.User)
                .Where(p => p.CreatedAt >= DateTime.UtcNow.AddHours(-24))
                .Where(p => friendIds.Contains(p.UserId) || p.UserId == user.Id)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return stories;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}