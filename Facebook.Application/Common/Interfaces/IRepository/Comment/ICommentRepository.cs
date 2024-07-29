using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository.Comment;

public interface ICommentRepository : IRepository<CommentEntity>
{
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByPostIdAsync(Guid postId);
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByUserIdAsync(Guid userId);
    Task<ErrorOr<Unit>> UpdateCommentAsync(CommentEntity comment);
}
