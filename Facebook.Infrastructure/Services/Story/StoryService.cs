using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Application.Common.Interfaces.Story.IService;
using Facebook.Domain.Story;

namespace Facebook.Infrastructure.Services.Story;

public class StoryService : IStoryService
{
    private readonly IStoryRepository _storyRepository;

    public StoryService(IStoryRepository storyRepository)
    {
        _storyRepository = storyRepository;
    }

    public async Task<ErrorOr<IEnumerable<StoryEntity>>> GetAllStoriesAsync()
    {
        return await _storyRepository.GetAllStoriesAsync();
    }

    public async Task<ErrorOr<StoryEntity>> GetStoryByIdAsync(Guid id)
    {
        return await _storyRepository.GetStoryByIdAsync(id);
    }

    public async Task<ErrorOr<Guid>> CreateStoryAsync(StoryEntity story)
    {
        story.CreatedAt = DateTime.Now;
        return await _storyRepository.CreateStoryAsync(story);
    }

    public async Task<ErrorOr<bool>> DeleteStoryAsync(Guid id)
    {
        var existingStoryResult = await _storyRepository.GetStoryByIdAsync(id);
        if (existingStoryResult.IsError)
        {
            return existingStoryResult.Errors;
        }
        var existingStory = existingStoryResult.Value;
        if (existingStory == null)
        {
            return false;
        }
        return await _storyRepository.DeleteStoryAsync(existingStory);
    }

    public Task<ErrorOr<StoryEntity>> SaveStoryAsync(StoryEntity story)
    {
        throw new NotImplementedException();
    }
}