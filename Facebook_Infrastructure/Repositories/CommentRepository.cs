using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Comment;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories;

public class CommentRepository(FacebookDbContext context) : Repository<CommentEntity>(context), ICommentRepository
{
    new public async Task<ErrorOr<CommentEntity>> SaveAsync(CommentEntity entity)
    {
        try
        {
            dbSet.Update(entity);
            await context.SaveChangesAsync();

            return dbSet.Include(c => c.UserEntity).Find(comment => comment.Id == entity.Id).FirstOrDefault() ?? entity;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByPostIdAsync(Guid postId)
    {
        var comments = await context.Comments
            .Include(c => c.UserEntity)
            .Include(c => c.ChildComments)
                .ThenInclude(cc => cc.UserEntity)
            .Where(comment => comment.PostId == postId)
            .ToListAsync();

        return comments ?? new List<CommentEntity>();
    }

    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByUserIdAsync(Guid userId)
    {
        var comment = await context.Comments.Where(comment => comment.UserId == userId).ToListAsync();
        if (!comment.Any())
        {
            return Error.NotFound();
        }
        return comment;
    }

    public async Task<ErrorOr<MediatR.Unit>> UpdateCommentAsync(CommentEntity comment)
    {
        try
        {
            var commentExist = await context.Comments.FindAsync(comment.Id);

            if (commentExist == null)
            {
                return Error.Failure("Comment not found");
            }

            commentExist.Message = comment.Message;

            context.Comments.Update(commentExist);
            await context.SaveChangesAsync();

            return MediatR.Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    new public async Task<ErrorOr<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var comment = await context.Comments
                .Include(c => c.ChildComments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null) return Error.NotFound();

            context.Comments.RemoveRange(comment.ChildComments);

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
