using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Post.Query.GetFriendsPosts;

public class GetFriendsPostsQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<GetFriendsPostsQuery, ErrorOr<PaginationResponse>>
{
    public async Task<ErrorOr<PaginationResponse>> Handle(GetFriendsPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = currentUserService.GetCurrentUserId();

            var result = await unitOfWork.Post.GetFriendsPostsAsync(new Guid(currentUserId), request.pageNumber, request.pageSize);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while receiving friends` posts: {ex.Message}");
            return Error.Failure($"Error while receiving friends` posts: {ex.Message}");
        }
    }
}
