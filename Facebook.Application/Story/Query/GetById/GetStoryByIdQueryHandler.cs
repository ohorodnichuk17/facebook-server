using Facebook.Domain.Story;
using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Domain.TypeExtensions;

namespace Facebook.Application.Story.Query.GetById;

public class GetStoryByIdQueryHandler : IRequestHandler<GetStoryByIdQuery, ErrorOr<StoryEntity>>
{
    private readonly IStoryRepository _storyRepository;

    public GetStoryByIdQueryHandler(IStoryRepository storyRepository)
    {
        _storyRepository = storyRepository;
    }

    public async Task<ErrorOr<StoryEntity>> Handle(GetStoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storyRepository.GetStoryByIdAsync(request.Id);

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
            Console.WriteLine($"Error retrieving story by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving story by id {request.Id}: {ex.Message}");
        }
    }
}