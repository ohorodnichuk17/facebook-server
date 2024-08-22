using ErrorOr;
using Facebook.Application.Post.Query.GetFriendsPosts;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository.Post;

public interface IPostRepository : IRepository<PostEntity>
{
    Task<ErrorOr<Unit>> UpdatePostAsync(PostEntity post);
    Task<ErrorOr<PostEntity>> GetPostByIdAsync(Guid requestPostId);
    Task<ErrorOr<List<PostEntity>>> SearchPostsByTags(string tag);
    Task<ErrorOr<PaginationResponse>> GetFriendsPostsAsync(Guid userId, int pageNumber, int pageSize);
}