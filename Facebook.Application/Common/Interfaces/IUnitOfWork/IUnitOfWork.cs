using Facebook.Application.Common.Interfaces.Chat.IRepository;
using Facebook.Application.Common.Interfaces.Action.IRepository;
using Facebook.Application.Common.Interfaces.Admin.IRepository;
using Facebook.Application.Common.Interfaces.Comment.IRepository;
using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Application.Common.Interfaces.Like.IRepository;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;

namespace Facebook.Application.Common.Interfaces.IUnitOfWork;

public interface IUnitOfWork
{
    IAdminRepository Admin { get; }
    IReactionRepository Reaction { get; }
    IFeelingRepository Feeling { get; }
    IStoryRepository Story { get; }
    IUserRepository User { get; }
    IUserProfileRepository UserProfile { get; }
    IPostRepository Post { get; }
    ILikeRepository Like { get; }
    IActionRepository Action { get; }
    ISubActionRepository SubAction { get; }
    ICommentRepository Comment { get; }
    IChatRepository Chat { get; }
    IMessageRepository Message { get; }
}