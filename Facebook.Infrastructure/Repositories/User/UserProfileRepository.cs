using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.User;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly FacebookDbContext _context;

    public UserProfileRepository(UserManager<UserEntity> userManager, FacebookDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ErrorOr<bool>> DeleteUserProfileAsync(string userId)
    {
        try
        {
            var deleteUser = await _userManager.FindByIdAsync(userId);
            var deleteUserProfile = await _context.UsersProfiles.SingleOrDefaultAsync(r => r.UserId.ToString() == userId);
            if (deleteUser == null)
            {
                return Error.Failure("User not found");
            }

            _context.Users.Remove(deleteUser);
            _context.UsersProfiles.Remove(deleteUserProfile);
            await _context.SaveChangesAsync();
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

        var user = await _context.UsersProfiles.SingleOrDefaultAsync(r => r.UserId.ToString() == userId);

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

        await _context.UsersProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync();

        return userProfile;
    }

    public async Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile, string name, string avatar)
    {
        var existProfile = await _context.UsersProfiles.SingleOrDefaultAsync(x => x.UserId == userProfile.UserId);
        var user = await _userManager.FindByIdAsync(userProfile.UserId.ToString());
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
        existProfile.City = userProfile.City;
        user.UserName = name;
        user.Avatar = avatar;

        _context.UsersProfiles.Update(existProfile);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return existProfile;
    }

    public async Task<ErrorOr<Unit>> BlockUserAsync(string userId)
    {
        try
        {
            var userToBlock = await _context.UsersProfiles.FindAsync(userId);

            if (userToBlock == null)
            {
                return Error.Failure("User not found");
            }

            userToBlock.IsBlocked = true;

            await _context.SaveChangesAsync();

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
            var userToUnBlock = await _context.UsersProfiles.FindAsync(userId);

            if (userToUnBlock == null)
            {
                return Error.Failure("User not found");
            }

            userToUnBlock.IsBlocked = false;

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
