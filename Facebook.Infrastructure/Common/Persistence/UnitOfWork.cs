using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.User;
using Facebook.Infrastructure.Repositories.Feeling;
using Facebook.Infrastructure.Repositories.Post;
using Facebook.Infrastructure.Repositories.Reaction;
using Facebook.Infrastructure.Repositories.Story;
using Facebook.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Infrastructure.Common.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private FacebookDbContext _context;
    private UserManager<UserEntity> _userManager;
    public IReactionRepository Reaction { get; private set; }
    public IFeelingRepository Feeling { get; private set; }
    public IStoryRepository Story { get; private set; }
    public IUserProfileRepository UserProfile { get; private set; }
    public IPostRepository Post { get; private set; }

    public UnitOfWork(FacebookDbContext context, UserManager<UserEntity> userManager)
    {
        _context = context;
        _userManager = userManager;
        Reaction = new ReactionRepository(_context);
        Feeling = new FeelingRepository(_context);
        Story = new StoryRepository(_context);
        UserProfile = new UserProfileRepository(_context, _userManager);
        Post = new PostRepository(_context);
    }
}