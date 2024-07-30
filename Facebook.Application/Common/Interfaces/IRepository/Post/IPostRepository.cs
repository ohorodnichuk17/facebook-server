using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository.Post;

public interface IPostRepository : IRepository<PostEntity>
{
    Task<ErrorOr<Unit>> UpdatePostAsync(PostEntity post);
    Task<ErrorOr<PostEntity>> GetPostByIdAsync(Guid requestPostId);
    Task<ErrorOr<List<PostEntity>>> SearchPostsByTags(string tag);
    Task<ErrorOr<IEnumerable<PostEntity>>> GetFriendsPostsAsync(Guid userId);
}