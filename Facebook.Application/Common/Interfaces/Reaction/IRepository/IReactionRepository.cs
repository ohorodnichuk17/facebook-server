using ErrorOr;
using Facebook.Domain.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Common.Interfaces.Reaction.IRepository;

public interface IReactionRepository
{
    Task<ErrorOr<ReactionEntity>> AddReactionAsync(ReactionEntity reaction);
    Task<ErrorOr<bool>> DeleteReactionAsync(Guid postId, Guid userId);
}
