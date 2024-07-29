using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Like;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
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
}
