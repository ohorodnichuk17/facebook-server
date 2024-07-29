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
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using Facebook.Infrastructure.Repositories;
using Facebook.Infrastructure.Repositories.Chat;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Infrastructure.Common.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private FacebookDbContext _context;
    private UserManager<UserEntity> _userManager;
    public IAdminRepository Admin { get; private set; }
    public IUserRepository User { get; private set; }
    public IReactionRepository Reaction { get; private set; }
    public IFeelingRepository Feeling { get; private set; }
    public IStoryRepository Story { get; private set; }
    public IUserProfileRepository UserProfile { get; private set; }
    public IPostRepository Post { get; private set; }
    public ILikeRepository Like { get; private set; }
    public ICommentRepository Comment { get; private set; }
    public IChatRepository Chat { get; private set; }
    public IMessageRepository Message { get; private set; }
    public IActionRepository Action { get; private set; }
    public ISubActionRepository SubAction { get; private set; }

    public UnitOfWork(FacebookDbContext context, UserManager<UserEntity> userManager)
    {
        _context = context;
        _userManager = userManager;
        Admin = new AdminRepository(_userManager, _context);
        User = new UserRepository(_userManager, _context);
        Reaction = new ReactionRepository(_context);
        Comment = new CommentRepository(_context);
        Like = new LikeRepository(_context);
        Feeling = new FeelingRepository(_context);
        Story = new StoryRepository(_context);
        UserProfile = new UserProfileRepository(_context, _userManager);
        Post = new PostRepository(_context);
        Action = new ActionRepository(_context);
        SubAction = new SubActionRepository(_context);
        Chat = new ChatRepository(_context);
        Message = new MessageRepository(_context);
    }
}