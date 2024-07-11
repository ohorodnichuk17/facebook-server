using Facebook.Domain.Post;
using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository;

namespace Facebook.Application.Common.Interfaces.Post.IRepository;

public interface IPostRepository : IRepository<PostEntity>
{
    Task<ErrorOr<Unit>> UpdatePostAsync(PostEntity post);
    Task<ErrorOr<PostEntity>> GetPostByIdAsync(Guid requestPostId);
}