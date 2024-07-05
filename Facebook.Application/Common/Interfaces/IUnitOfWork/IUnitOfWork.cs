using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;

namespace Facebook.Application.Common.Interfaces.IUnitOfWork;

public interface IUnitOfWork
{
    IReactionRepository Reaction { get; }
    IFeelingRepository Feeling { get; }
    IStoryRepository Story { get; }
    IUserProfileRepository UserProfile { get; }
    IPostRepository Post { get; }
}