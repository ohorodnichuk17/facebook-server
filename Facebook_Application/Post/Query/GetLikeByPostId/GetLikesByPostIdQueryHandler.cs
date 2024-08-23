using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetLikeByPostId;

public class GetLikesByPostIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetLikesByPostIdQuery, ErrorOr<IEnumerable<LikeEntity>>>
{
    public async Task<ErrorOr<IEnumerable<LikeEntity>>> Handle(GetLikesByPostIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Like.GetLikesByPostIdAsync(request.PostId);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while receiving likes: {ex.Message}");
            return Error.Failure($"Error while receiving likes: {ex.Message}");
        }
    }
}
