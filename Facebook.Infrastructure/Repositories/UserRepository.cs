using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Facebook.Infrastructure.Persistance;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    
    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<ErrorOr<List<User>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<User>> GetUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return Error.Failure("Invalid userId");
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return Error.Failure("User not found");
        }

        return user;
    }
    
    public async Task<ErrorOr<User>> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Error.Failure("User not found");
        }

        return user;
    }

    public async Task<ErrorOr<User>> CreateUserAsync(User user, string password, string role)
    {
        user.UserName = $"{user.FirstName}{user.LastName}".ToLower();

        var createUserResult = await _userManager.CreateAsync(user, password);

        if (!createUserResult.Succeeded)
        {
            foreach (var error in createUserResult.Errors)
            {
                Console.WriteLine($"Error creating user: {error.Description}");
            }
            return Error.Failure("Error creating user");
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, role);

        if (!addToRoleResult.Succeeded)
        {
            foreach (var error in addToRoleResult.Errors)
            {
                Console.WriteLine($"Error adding user to role: {error.Description}");
            }
            return Error.Failure("Error adding user to role");
        }

        return user;
    }


    public async Task<ErrorOr<Unit>> DeleteUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return Error.Failure("Invalid userId");
        }

        var userToDelete = await _userManager.FindByIdAsync(userId.ToString());
        if (userToDelete == null)
        {
            return Error.Failure("User not found");
        }

        var result = await _userManager.DeleteAsync(userToDelete);
        if (!result.Succeeded)
        {
            return Error.Failure("Failed to delete user");
        }

        return Unit.Value;
    }

    public async Task<ErrorOr<Unit>> UpdateUserAsync(User user, string? newPassword)
    {
        if (user == null)
        {
            return Error.Failure("User cannot be null");
        }

        var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (existingUser == null)
        {
            return Error.Failure("User not found");
        }

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.Birthday = user.Birthday;
        existingUser.Biography = user.Biography;
        existingUser.Gender = user.Gender;
        existingUser.CoverPhoto = user.CoverPhoto;
        existingUser.ProfilePicture = user.ProfilePicture;
        existingUser.IsProfilePublic = user.IsProfilePublic;
        
        if (!string.IsNullOrEmpty(newPassword))
        {
            var passwordChangeResult = await _userManager.ChangePasswordAsync(existingUser, existingUser.PasswordHash, newPassword);
            if (!passwordChangeResult.Succeeded)
            {
                return Error.Failure("Failed to update password");
            }
        }

        
        var result = await _userManager.UpdateAsync(existingUser);
        if (!result.Succeeded)
        {
            return Error.Failure("Failed to update user");
        }

        return Unit.Value;
    }


    public async Task<ErrorOr<Unit>> BlockUserAsync(Guid userId)
    {
        try
        {
            var userToBlock = await _userManager.FindByIdAsync(userId.ToString());

            if (userToBlock == null)
            {
                return Error.Failure("User not found");
            }

            userToBlock.IsBlocked = true;

            var result = await _userManager.UpdateAsync(userToBlock);

            if (!result.Succeeded)
            {
                return Error.Failure("Failed to block user");
            }
            
            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> UnblockUserAsync(Guid userId)
    {
        try
        {
            var userToUnBlock = await _userManager.FindByIdAsync(userId.ToString());

            if (userToUnBlock == null)
            {
                return Error.Failure("User not found");
            }

            userToUnBlock.IsBlocked = false;

            var result = await _userManager.UpdateAsync(userToUnBlock);

            if (!result.Succeeded)
            {
                return Error.Failure("Failed to block user");
            }
            
            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }   
    }

    public async Task<ErrorOr<List<User>>> SearchUsersAsync(string? firstName, string? lastName)
    {
        try
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return Error.Failure("No search criteria provided");
            }

            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(u => u.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(u => u.LastName.Contains(lastName));
            }

            var searchResult = await query.ToListAsync();

            if (searchResult.Count == 0)
            {
                return Error.Failure("No users found");
            }

            return searchResult;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
    
    public async Task<List<User>> GetFriendsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> SendFriendRequestAsync(Guid userId, Guid friendId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> AcceptFriendRequestAsync(Guid userId, Guid friendRequestId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> RejectFriendRequestAsync(Guid userId, Guid friendRequestId)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetBlockedUsersAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> BlockUserByUserAsync(Guid userId, Guid blockedUserId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> UnblockUserByUserAsync(Guid userId, Guid blockedUserId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> SetProfilePrivacyAsync(Guid userId, bool isProfilePublic)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<bool>> GetProfilePrivacyAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}