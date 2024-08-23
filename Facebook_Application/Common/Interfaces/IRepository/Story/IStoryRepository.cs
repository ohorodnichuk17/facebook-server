using ErrorOr;
using Facebook.Domain.Story;

namespace Facebook.Application.Common.Interfaces.IRepository.Story;

public interface IStoryRepository : IRepository<StoryEntity>
{
    Task<ErrorOr<IEnumerable<StoryEntity>>> GetFriendsStoriesAsync(Guid userId);
}