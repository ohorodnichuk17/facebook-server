using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetFriendsPosts;

public class PaginationResponse
{
    public int TotalCount { get; set; }
    public List<PostEntity> Posts { get; set; }
}

public record GetFriendsPostsQuery(int pageNumber, int pageSize) : IRequest<ErrorOr<PaginationResponse>>;