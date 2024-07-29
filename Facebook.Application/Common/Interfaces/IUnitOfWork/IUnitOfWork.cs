using Facebook.Application.Common.Interfaces.IRepository.Action;
using Facebook.Application.Common.Interfaces.IRepository.Admin;
using Facebook.Application.Common.Interfaces.IRepository.Chat;
using Facebook.Application.Common.Interfaces.IRepository.Comment;
using Facebook.Application.Common.Interfaces.IRepository.Feeling;
using Facebook.Application.Common.Interfaces.IRepository.Like;
using Facebook.Application.Common.Interfaces.IRepository.Post;
using Facebook.Application.Common.Interfaces.IRepository.Reaction;
using Facebook.Application.Common.Interfaces.IRepository.Story;
using Facebook.Application.Common.Interfaces.IRepository.User;

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