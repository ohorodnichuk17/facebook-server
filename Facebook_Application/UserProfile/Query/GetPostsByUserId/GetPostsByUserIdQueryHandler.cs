using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.UserProfile.Query.GetPostsByUserId;

public class GetPostsByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPostsByUserIdQuery, ErrorOr<IEnumerable<PostEntity>>>
{
    public async Task<ErrorOr<IEnumerable<PostEntity>>> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var posts = await unitOfWork.UserProfile.GetPostsByUserIdAsync(request.UserId);

            if (posts.IsError)
            {
                return Error.Failure(posts.Errors.ToString() ?? string.Empty);
            }

            return posts;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
