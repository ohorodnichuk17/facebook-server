using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.UserProfile.Query.GetStoriesByUserId;

public class GetStoriesByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetStoriesByUserIdQuery, ErrorOr<IEnumerable<StoryEntity>>>
{
    public async Task<ErrorOr<IEnumerable<StoryEntity>>> Handle(GetStoriesByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var stories = await unitOfWork.UserProfile.GetStoriesByUserIdAsync(request.UserId);

            if (stories.IsError)
            {
                return Error.Failure(stories.Errors.ToString() ?? string.Empty);
            }

            return stories;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
