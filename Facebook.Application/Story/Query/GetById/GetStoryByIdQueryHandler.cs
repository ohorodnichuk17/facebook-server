using Facebook.Domain.Story;
using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IService;
using Facebook.Domain.TypeExtensions;

namespace Facebook.Application.Story.Query.GetById;

public class GetStoryByIdQueryHandler : IRequestHandler<GetStoryByIdQuery, ErrorOr<StoryEntity>>
{
    private readonly IStoryService _storyService;

    public GetStoryByIdQueryHandler(IStoryService storyService)
    {
        _storyService = storyService;
    }

    public async Task<ErrorOr<StoryEntity>> Handle(GetStoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _storyService.GetStoryByIdAsync(request.Id);

        if (result.IsSuccess())
        {
            return result.Value;
        }
        else
        {
            return Error.Failure("Error");
        }
    }
}