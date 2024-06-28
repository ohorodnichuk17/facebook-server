using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Facebook.Infrastructure.Repositories.User;

public class UserProfileRepository(UserManager<UserEntity> userManager, FacebookDbContext context)
    : IUserProfileRepository
{
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

    public async Task<ErrorOr<UserProfileEntity>> UserCreateProfileAsync(Guid userId)
    {
        UserProfileEntity userProfile = new UserProfileEntity()
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

    public async Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile, 
        string firstName,  string lastName, string avatar)
    {
        var existProfile = await context.UsersProfiles.SingleOrDefaultAsync(x => x.UserId == userProfile.UserId);
        var user = await userManager.FindByIdAsync(userProfile.UserId.ToString());
        if (existProfile == null || user == null)
        {
            return Error.Failure("Profile not found!");
        }

        if (userProfile.Biography != null)
        {
            existProfile.Biography = userProfile.Biography;
        }
        if (userProfile.CoverPhoto != null)
        {
            existProfile.CoverPhoto = userProfile.CoverPhoto;
        }
        existProfile.IsProfilePublic = userProfile.IsProfilePublic;
        existProfile.IsBlocked = userProfile.IsBlocked;
        if (userProfile.Region != null)
        {
            existProfile.Region = userProfile.Region;
        }
        if (userProfile.Country != null)
        {
            existProfile.Country = userProfile.Country;
        }
        if (userProfile.Pronouns != null)
        {
            existProfile.Pronouns = userProfile.Pronouns;
        }
        if (firstName != null)
        {
            user.FirstName = firstName;
        }
        if (lastName != null)
        {
            user.LastName = lastName;
        }
        if (avatar != null)
        {
            user.Avatar = avatar;
        }

        context.UsersProfiles.Update(existProfile);
        context.Users.Update(user);
        await context.SaveChangesAsync();

        return existProfile;
    }

    public async Task<ErrorOr<MediatR.Unit>> BlockUserAsync(string userId)
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

            return MediatR.Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<MediatR.Unit>> UnblockUserAsync(string userId)
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

            return MediatR.Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
