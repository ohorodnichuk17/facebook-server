using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository.Comment;

public interface ICommentRepository : IRepository<CommentEntity>
{
    new Task<ErrorOr<CommentEntity>> SaveAsync(CommentEntity entity);
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByPostIdAsync(Guid postId);
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByUserIdAsync(Guid userId);
    Task<ErrorOr<Unit>> UpdateCommentAsync(CommentEntity comment);
    new Task<ErrorOr<bool>> DeleteAsync(Guid id);
}
