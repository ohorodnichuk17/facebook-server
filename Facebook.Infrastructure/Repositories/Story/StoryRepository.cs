using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Domain.Story;
using Facebook.Infrastructure.Common.Persistence;

namespace Facebook.Infrastructure.Repositories.Story;

public class StoryRepository(FacebookDbContext context) : Repository<StoryEntity>(context), IStoryRepository
{
}