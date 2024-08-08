using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Like;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories;

public class LikeRepository(FacebookDbContext context) : Repository<LikeEntity>(context), ILikeRepository
{
    public async Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikesByPostIdAsync(Guid postId)
    {
        var like = await context.Likes.Where(like => like.PostId == postId).ToListAsync();
        if (!like.Any())
        {
            return Error.NotFound();
        }
        return like;
    }

    public async Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikesByUserIdAsync(Guid userId)
    {
        var like = await context.Likes.Where(like => like.UserId == userId).ToListAsync();
        if (!like.Any())
        {
            return Error.NotFound();
        }
        return like;
    }

    public async Task<ErrorOr<LikeEntity>> SaveIfNotExist(LikeEntity entity)
    {
        var existLike = await context.Likes
        .FirstOrDefaultAsync(like => like.UserId == entity.UserId && like.PostId == entity.PostId);

        if (existLike != null)
        {
            return Error.Failure("Like already exists.");
        }

        context.Likes.Add(entity);
        await context.SaveChangesAsync();

        return entity;
    }
    public async Task<ErrorOr<bool>> DeleteByPostId(Guid userId, Guid postId)
    {
        var like = await context.Likes
        .FirstOrDefaultAsync(like => like.UserId == userId && like.PostId == postId);

        if (like == null)
        {
            return Error.NotFound("Like not found");
        }

        context.Likes.Remove(like);
        await context.SaveChangesAsync();

        return true;
    }
}
