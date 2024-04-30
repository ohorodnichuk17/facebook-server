using ErrorOr;
using Facebook.Domain.Story;

namespace Facebook.Application.Common.Interfaces.Story.IService;

public interface IStoryService
{
    Task<ErrorOr<IEnumerable<StoryEntity>>> GetAllStoriesAsync();
    Task<ErrorOr<StoryEntity>> GetStoryByIdAsync(Guid id);
    Task<ErrorOr<Guid>> CreateStoryAsync(StoryEntity story);
    Task<ErrorOr<bool>> DeleteStoryAsync(Guid id);
    Task<ErrorOr<StoryEntity>> SaveStoryAsync(StoryEntity story);
}