using ErrorOr;
using Facebook.Application.Common.Interfaces.Comment.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Infrastructure.Repositories.Comment;

public class CommentRepository(FacebookDbContext context) : Repository<CommentEntity>(context), ICommentRepository
{
    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentByPostIdAsync(Guid postId)
    {
        var comment = await context.Comments.Where(comment => comment.PostId == postId).ToListAsync();
        if (!comment.Any())
        {
            return Error.NotFound();
        }
        return comment;
    }

    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentByUserIdAsync(Guid userId)
    {
        var comment = await context.Comments.Where(comment => comment.UserId == userId).ToListAsync();
        if (!comment.Any())
        {
            return Error.NotFound();
        }
        return comment;
    }
}
