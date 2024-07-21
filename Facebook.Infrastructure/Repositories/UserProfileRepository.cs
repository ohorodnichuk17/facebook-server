using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.User;

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
            IsBlocked = false,
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
            var deleteUserProfile = await context.UsersProfiles.SingleOrDefaultAsync(r => r.UserId.ToString() == userId);
            if (deleteUser == null)
            {
                return Error.Failure("User not found");
            }

            context.Users.Remove(deleteUser);
            context.UsersProfiles.Remove(deleteUserProfile);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile,
       string? firstName, string? lastName, string? avatar)
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
        existProfile.IsBlocked = userProfile.IsBlocked;
        existProfile.Region = userProfile.Region;
        existProfile.Pronouns = userProfile.Pronouns;
        existProfile.Country = userProfile.Country;

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
        existProfile.IsBlocked = userProfile.IsBlocked;
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

        var user = await context.UsersProfiles.SingleOrDefaultAsync(r => r.UserId.ToString() == userId);

        if (user == null)
        {
            return Error.Failure("User not found");
        }
        return user;
    }

    public async Task<ErrorOr<Unit>> BlockUserAsync(string userId)
    {
        try
        {
            var userToBlock = await context.UsersProfiles.FindAsync(userId);

            if (userToBlock == null)
            {
                return Error.Failure("User not found");
            }

            userToBlock.IsBlocked = true;

            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> UnblockUserAsync(string userId)
    {
        try
        {
            var userToUnBlock = await context.UsersProfiles.FindAsync(userId);

            if (userToUnBlock == null)
            {
                return Error.Failure("User not found");
            }

            userToUnBlock.IsBlocked = false;

            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}