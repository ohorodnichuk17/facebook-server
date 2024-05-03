using ErrorOr;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Common.Interfaces.Story.IRepository;

public interface IStoryRepository
{
    Task<ErrorOr<IEnumerable<StoryEntity>>> GetAllStoriesAsync();
    Task<ErrorOr<StoryEntity>> GetStoryByIdAsync(Guid id);
    Task<ErrorOr<Guid>> CreateStoryAsync(StoryEntity story);
    Task<ErrorOr<Unit>> UpdateStoryAsync(StoryEntity story);
    Task<ErrorOr<bool>> DeleteStoryAsync(Guid storyId);
    Task<ErrorOr<Unit>> SaveStoryAsync(StoryEntity story);

}