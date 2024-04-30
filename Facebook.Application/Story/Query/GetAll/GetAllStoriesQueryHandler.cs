using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IService;
using Facebook.Domain.Story;
using Facebook.Domain.TypeExtensions;

namespace Facebook.Application.Story.Query.GetAll;

public class GetAllStoriesQueryHandler  : IRequestHandler<GetAllStoriesQuery, ErrorOr<IEnumerable<StoryEntity>>>
{
    private readonly IStoryService _storyService;

    public GetAllStoriesQueryHandler(IStoryService storyService)
    {
        _storyService = storyService;
    }

    public async Task<ErrorOr<IEnumerable<StoryEntity>>> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _storyService.GetAllStoriesAsync();

        if (result.IsSuccess())
        {
            return result;
        }
        else
        {
            return Error.Failure("Error");
        }
    }
}