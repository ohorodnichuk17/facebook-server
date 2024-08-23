using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Story;
using Facebook.Domain.TypeExtensions;

namespace Facebook.Application.Story.Query.GetAll;

public class GetAllStoriesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllStoriesQuery, ErrorOr<IEnumerable<StoryEntity>>>
{
    public async Task<ErrorOr<IEnumerable<StoryEntity>>> Handle(GetAllStoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Story.GetAllAsync();

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