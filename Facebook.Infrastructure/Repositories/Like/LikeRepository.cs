using ErrorOr;
using Facebook.Application.Common.Interfaces.Like.IRepository;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Infrastructure.Migrations;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Infrastructure.Repositories.Like;

public class LikeRepository(FacebookDbContext context) : Repository<LikeEntity>(context), ILikeRepository
{
    public async Task<ErrorOr<bool>> AddLikeAsync(LikeEntity like)
    {
        await context.Likes.AddAsync(like);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<ErrorOr<bool>> DeleteLikeAsync(Guid id)
    {
        var like = await context.Likes.FindAsync(id);
        if (like != null)
        {
            context.Likes.Remove(like);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<ErrorOr<LikeEntity>> GetLikeByIdAsync(Guid id)
    {
        var like = await context.Likes.FindAsync(id);
        if (like == null)
        {
            return Error.NotFound();
        }
        return like;
    }

    public async Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikeByPostIdAsync(Guid postId)
    {
        var like = await context.Likes.Where(like => like.PostId == postId).ToListAsync();
        if (like == null)
        {
            return Error.NotFound();
        }
        return like;
    }

    public async Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikeByUserIdAsync(Guid userId)
    {
        var like = await context.Likes.Where(like => like.UserId == userId).ToListAsync();
        if (like == null)
        {
            return Error.NotFound();
        }
        return like;
    }
}
