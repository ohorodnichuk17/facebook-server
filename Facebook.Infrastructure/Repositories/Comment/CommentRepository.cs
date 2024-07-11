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
    public async Task<ErrorOr<bool>> AddCommentAsync(CommentEntity comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<ErrorOr<bool>> DeleteCommentAsync(Guid id)
    {
        var comment = await context.Comments.FindAsync(id);
        if (comment != null)
        {
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<ErrorOr<CommentEntity>> GetCommentByIdAsync(Guid id)
    {
        var comment = await context.Comments.FindAsync(id);
        if (comment == null)
        {
            return Error.NotFound();
        }
        return comment;
    }

    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentByPostIdAsync(Guid postId)
    {
        var comment = await context.Comments.Where(comment => comment.PostId == postId).ToListAsync();
        if (comment == null)
        {
            return Error.NotFound();
        }
        return comment;
    }

    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentByUserIdAsync(Guid userId)
    {
        var comment = await context.Comments.Where(comment => comment.UserId == userId).ToListAsync();
        if (comment == null)
        {
            return Error.NotFound();
        }
        return comment;
    }
}
