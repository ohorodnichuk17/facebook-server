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
    
    public DbSet<FriendRequestEntity> FriendRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<StoryEntity>()
            .HasOne<UserEntity>()
            .WithMany(u => u.Stories)
            .HasForeignKey(s => s.UserId);
        
        builder.Entity<ImagesEntity>()
            .HasOne(i => i.Post)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.PostId);
        
        builder.Entity<PostEntity>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId);
        
        builder.Entity<UserProfileEntity>()
            .HasOne(up => up.UserEntity)
            .WithOne()
            .HasForeignKey<UserProfileEntity>(up => up.UserId);
        
        builder.Entity<FriendRequestEntity>()
            .HasOne(fr => fr.Sender)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(fr => fr.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FriendRequestEntity>()
            .HasOne(fr => fr.Receiver)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(fr => fr.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}