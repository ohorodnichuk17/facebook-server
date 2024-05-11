using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Domain.Story;
using Facebook.Domain.TypeExtensions;

namespace Facebook.Application.Story.Query.GetAll;

public class GetAllStoriesQueryHandler  : IRequestHandler<GetAllStoriesQuery, ErrorOr<IEnumerable<StoryEntity>>>
{
    private readonly IStoryRepository _storyRepository;

    public GetAllStoriesQueryHandler(IStoryRepository storyRepository)
    {
        _storyRepository = storyRepository;
    }

    public async Task<ErrorOr<IEnumerable<StoryEntity>>> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storyRepository.GetAllStoriesAsync();

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while receiving stories: {ex.Message}");
            return Error.Failure($"Error while receiving stories: {ex.Message}");
        }
        
    }
}