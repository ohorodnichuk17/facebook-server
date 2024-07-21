using ErrorOr;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Reaction;

public class ReactionRepository(FacebookDbContext context)
    : Repository<ReactionEntity>(context), IReactionRepository
{
    public async Task<ErrorOr<IEnumerable<ReactionEntity>>> GetReactionsByPostIdAsync(Guid postId)
    {
        var reaction = await context.Reactions.Where(reaction => reaction.PostId == postId).ToListAsync();
        if (!reaction.Any())
        {
            return Error.NotFound();
        }
        return reaction;
    }

    public async Task<ErrorOr<IEnumerable<ReactionEntity>>> GetReactionsByUserIdAsync(Guid userId)
    {
        var reaction = await context.Reactions.Where(reaction => reaction.UserId == userId).ToListAsync();
        if (!reaction.Any())
        {
            return Error.NotFound();
        }
        return reaction;
    }
}
