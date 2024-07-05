using ErrorOr;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Reaction;

public class ReactionRepository(FacebookDbContext context)
    : Repository<ReactionEntity>(context), IReactionRepository
{
    public async Task<ErrorOr<bool>> DeleteReactionAsync(Guid postId, Guid userId)
    {
        try
        {
            var reaction = await context.Reactions.SingleOrDefaultAsync(u => u.UserId == userId && u.PostId == postId);
            if (reaction == null)
            {
                return Error.Failure("Error");
            }

            context.Reactions.Remove(reaction);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
