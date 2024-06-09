using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Common.Persistence;

public class FacebookDbContext 
    : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public FacebookDbContext() : base() { }
    public FacebookDbContext(DbContextOptions<FacebookDbContext> options) : base(options) { }

    public DbSet<StoryEntity> Stories { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<ImagesEntity> Images { get; set; }
    public DbSet<UserProfileEntity> UsersProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}