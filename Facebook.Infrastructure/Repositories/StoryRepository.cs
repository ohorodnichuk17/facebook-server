using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;
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
}