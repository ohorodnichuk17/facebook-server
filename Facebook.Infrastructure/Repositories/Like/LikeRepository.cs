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
