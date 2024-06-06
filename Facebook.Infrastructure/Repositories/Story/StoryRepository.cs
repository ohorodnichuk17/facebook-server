using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Domain.Story;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Story;

public class StoryRepository : IStoryRepository
{
    private readonly FacebookDbContext _context;

    public StoryRepository(FacebookDbContext context)
    {
        _context = context;
    }
    
    public async Task<ErrorOr<IEnumerable<StoryEntity>>> GetAllStoriesAsync()
    {
        return await _context.Stories.ToListAsync();
    }
    
    public async Task<ErrorOr<StoryEntity>> GetStoryByIdAsync(Guid id)
    {
        var story = await _context.Stories.FindAsync(id);

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
            _context.Stories.Add(story);
            await _context.SaveChangesAsync();
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
            var existingStory = await _context.Stories.FindAsync(story.Id);

            if (existingStory == null)
            {
                return Error.Failure("Story not found");
            }

            existingStory.Content = story.Content;
            existingStory.Image = story.Image;

            _context.Stories.Update(existingStory);
            await _context.SaveChangesAsync();
        
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
            var story = await _context.Stories.FindAsync(storyId);
            if (story == null)
            {
                return Error.Failure("Story not found");
            }

            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> SaveStoryAsync(StoryEntity story)
    {
        await _context.SaveChangesAsync();
        return Unit.Value;
    }
}