using Facebook.Domain.Post;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Common.Interfaces.Post.IRepository;

public interface IPostRepository
{
    Task<ErrorOr<IEnumerable<PostEntity>>> GetAllPostsAsync();
    Task<ErrorOr<PostEntity>> GetPostByIdAsync(Guid id);
    Task<ErrorOr<Guid>> CreatePostAsync(PostEntity post);
    Task<ErrorOr<Unit>> UpdatePostAsync(PostEntity post);
    Task<ErrorOr<bool>> DeletePostAsync(Guid postId);
    Task<ErrorOr<Guid>> SavePostAsync(PostEntity post);
}