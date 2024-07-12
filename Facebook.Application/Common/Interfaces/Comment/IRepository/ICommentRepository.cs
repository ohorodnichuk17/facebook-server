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
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByPostIdAsync(Guid postId);
    Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByUserIdAsync(Guid userId);
}
