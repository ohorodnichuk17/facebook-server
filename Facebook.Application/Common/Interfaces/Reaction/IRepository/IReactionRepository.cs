using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository;
using Facebook.Domain.Post;

namespace Facebook.Application.Common.Interfaces.Reaction.IRepository;

public interface IReactionRepository : IRepository<ReactionEntity>
{
    Task<ErrorOr<IEnumerable<ReactionEntity>>> GetReactionsByPostIdAsync(Guid postId);
    Task<ErrorOr<IEnumerable<ReactionEntity>>> GetReactionsByUserIdAsync(Guid userId);
}
