using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Common.Interfaces.Comment.IRepository;

public interface ICommentRepository : IRepository<CommentEntity>
{
    Task<ErrorOr<CommentEntity>> GetCommentByIdAsync(Guid id);
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentByPostIdAsync(Guid postId);
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentByUserIdAsync(Guid userId);
    Task<ErrorOr<bool>> AddCommentAsync(CommentEntity like);
    Task<ErrorOr<bool>> DeleteCommentAsync(Guid id);
}
