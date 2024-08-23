using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.User;
using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories;

public class UserProfileRepository(
   FacebookDbContext context,
   UserManager<UserEntity> userManager)
   : Repository<UserProfileEntity>(context), IUserProfileRepository
{
    public async Task<ErrorOr<UserProfileEntity>> UserCreateProfileAsync(Guid userId)
    {
        UserProfileEntity userProfile = new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            IsProfilePublic = true
        };

        await context.UsersProfiles.AddAsync(userProfile);
        await context.SaveChangesAsync();

        return userProfile;
    }

    public async Task<ErrorOr<bool>> DeleteUserProfileAsync(string userId)
    {
        try
        {
            var deleteUser = await userManager.FindByIdAsync(userId);
            if (deleteUser == null)
            {
                return Error.Failure("User not found");
            }

            var messages = await context.Messages.Where(m => m.UserId.ToString() == userId).ToListAsync();
            context.Messages.RemoveRange(messages);

            var friendRequests = await context.FriendRequests
                .Where(fr => fr.SenderId.ToString() == userId || fr.ReceiverId.ToString() == userId)
                .ToListAsync();
            context.FriendRequests.RemoveRange(friendRequests);

            var deleteUserProfile = await context.UsersProfiles.SingleOrDefaultAsync(r => r.UserId.ToString() == userId);
            context.UsersProfiles.Remove(deleteUserProfile);
            context.Users.Remove(deleteUser);

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
    public async Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile,
       string? firstName, string? lastName, string? avatar, bool isOnline)
    {
        var existProfile = await context.UsersProfiles.SingleOrDefaultAsync(x => x.UserId == userProfile.UserId);
        var user = await userManager.FindByIdAsync(userProfile.UserId.ToString());
        if (existProfile == null || user == null)
        {
            return Error.Failure("Profile not found!");
        }

        existProfile.Biography = userProfile.Biography;
        existProfile.CoverPhoto = userProfile.CoverPhoto;
        existProfile.IsProfilePublic = userProfile.IsProfilePublic;
        existProfile.Region = userProfile.Region;
        existProfile.Pronouns = userProfile.Pronouns;
        existProfile.Country = userProfile.Country;

        user.IsOnline = isOnline;

        if (!string.IsNullOrEmpty(firstName))
        {
            user.FirstName = firstName;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            user.LastName = lastName;
        }

        if (!string.IsNullOrEmpty(avatar))
        {
            user.Avatar = avatar;
        }

        context.UsersProfiles.Update(existProfile);
        context.Users.Update(user);
        await context.SaveChangesAsync();

        return existProfile;
    }

    public async Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile)
    {
        var existProfile = await context.UsersProfiles.SingleOrDefaultAsync(x => x.UserId == userProfile.UserId);
        var user = await userManager.FindByIdAsync(userProfile.UserId.ToString());
        if (existProfile == null || user == null)
        {
            return Error.Failure("Profile not found!");
        }

        existProfile.Biography = userProfile.Biography;
        existProfile.CoverPhoto = userProfile.CoverPhoto;
        existProfile.IsProfilePublic = userProfile.IsProfilePublic;
        existProfile.Region = userProfile.Region;
        existProfile.Country = userProfile.Country;

        context.UsersProfiles.Update(existProfile);
        await context.SaveChangesAsync();

        return existProfile;
    }

    public async Task<ErrorOr<UserProfileEntity>> GetUserProfileByIdAsync(string userId)
    {
        if (userId == null)
        {
            return Error.Failure("Invalid userId");
        }

        var user = await context.UsersProfiles
                .Include(up => up.UserEntity)
                .SingleOrDefaultAsync(r => r.UserId.ToString() == userId);

        if (user == null)
        {
            return Error.Failure("User not found");
        }
        return user;
    }

    public async Task<ErrorOr<IEnumerable<PostEntity>>> GetPostsByUserIdAsync(Guid userId)
    {
        try
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                return Error.NotFound();
            }

            var posts = await context.Posts
                .Include(p => p.Action)
                .Include(p => p.SubAction)
                .Include(p => p.Feeling)
                .Include(p => p.Images)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            return posts;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<StoryEntity>>> GetStoriesByUserIdAsync(Guid userId)
    {
        try
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                return Error.NotFound();
            }

            var stories = await context.Stories
                .Include(p => p.User)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            return stories;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}