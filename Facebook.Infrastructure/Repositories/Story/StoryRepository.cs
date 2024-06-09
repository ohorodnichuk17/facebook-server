using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Domain.Story;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Story;

public class StoryRepository(FacebookDbContext context) : IStoryRepository
{
    public async Task<ErrorOr<IEnumerable<StoryEntity>>> GetAllStoriesAsync()
    {
        return await context.Stories.ToListAsync();
    }
    
    public async Task<ErrorOr<StoryEntity>> GetStoryByIdAsync(Guid id)
    {
        var story = await context.Stories.FindAsync(id);

        if (story == null)
        {
            return Error.Failure("Story not found");
        }

        return story;
    }

    public async Task<ErrorOr<Guid>> CreateStoryAsync(StoryEntity story)
    {
        try
        {
            context.Stories.Add(story);
            await context.SaveChangesAsync();
            return story.Id;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> UpdateStoryAsync(StoryEntity story)
    {
        try
        {
            var existingStory = await context.Stories.FindAsync(story.Id);

            if (existingStory == null)
            {
                return Error.Failure("Story not found");
            }

            existingStory.Content = story.Content;
            existingStory.Image = story.Image;

            context.Stories.Update(existingStory);
            await context.SaveChangesAsync();
        
            return Unit.Value; 
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message); 
        }
    }

    public async Task<ErrorOr<bool>> DeleteStoryAsync(Guid storyId)
    {
        try
        {
            var story = await context.Stories.FindAsync(storyId);
            if (story == null)
            {
                return Error.Failure("Story not found");
            }

            context.Stories.Remove(story);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> SaveStoryAsync(StoryEntity story)
    {
        await context.SaveChangesAsync();
        return Unit.Value;
    }
}