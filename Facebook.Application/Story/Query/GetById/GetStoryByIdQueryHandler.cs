using Facebook.Domain.Story;
using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Domain.TypeExtensions;

namespace Facebook.Application.Story.Query.GetById;

public class GetStoryByIdQueryHandler(IStoryRepository storyRepository)
    : IRequestHandler<GetStoryByIdQuery, ErrorOr<StoryEntity>>
{
    public async Task<ErrorOr<StoryEntity>> Handle(GetStoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await storyRepository.GetStoryByIdAsync(request.Id);

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