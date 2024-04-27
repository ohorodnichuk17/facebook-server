using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.UserEntity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Facebook.Infrastructure.Persistance;

public class UserRepository : IUserRepository
{
    private readonly UserManager<UserEntity> _userManager;
    
    public UserRepository(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }
    
    // TEMP
    public async Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity userEntity, string password, string role)
    {
        userEntity.UserName = $"{userEntity.Email}".ToLower();

        var createUserResult = await _userManager.CreateAsync(userEntity, password);

        if (!createUserResult.Succeeded)
        {
            foreach (var error in createUserResult.Errors)
            {
                Console.WriteLine($"Error creating user: {error.Description}");
            }
            return Error.Failure("Error creating user");
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(userEntity, role);

        if (!addToRoleResult.Succeeded)
        {
            foreach (var error in addToRoleResult.Errors)
            {
                Console.WriteLine($"Error adding user to role: {error.Description}");
            }
            return Error.Failure("Error adding user to role");
        }

        return userEntity;
    }
    
    public async Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync()
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

    public async Task<ErrorOr<UserEntity>> GetUserByIdAsync(string userId)
    {
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "userId cannot be null");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Error.NotFound();
        }

        return user;
    }
    
    public async Task<ErrorOr<UserEntity>> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Error.Failure("User not found");
        }

        return user;
    }

    public async Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity userEntity, string password)
    {
        userEntity.UserName = $"{userEntity.Email}".ToLower();

        var createUserResult = await _userManager.CreateAsync(userEntity, password);

        if (!createUserResult.Succeeded)
        {
            foreach (var error in createUserResult.Errors)
            {
                Console.WriteLine($"Error creating user: {error.Description}");
            }
            return Error.Failure("Error creating user");
        }

        return userEntity;
    }


    public async Task<ErrorOr<Unit>> DeleteUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
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

    public async Task<ErrorOr<List<string>>> FindRolesByUserIdAsync(UserEntity userEntity)
    {
        var roles = await _userManager.GetRolesAsync(userEntity);
        if (roles == null)
        {
            return Error.NotFound();
        }

        return roles.ToList();
    }

    // public async Task<ErrorOr<Unit>> BlockUserAsync(string userId)
    // {
    //     try
    //     {
    //         var userToBlock = await _userManager.FindByIdAsync(userId.ToString());
    //
    //         if (userToBlock == null)
    //         {
    //             return Error.Failure("User not found");
    //         }
    //
    //         userToBlock.IsBlocked = true;
    //
    //         var result = await _userManager.UpdateAsync(userToBlock);
    //
    //         if (!result.Succeeded)
    //         {
    //             return Error.Failure("Failed to block user");
    //         }
    //         
    //         return Unit.Value;
    //     }
    //     catch (Exception ex)
    //     {
    //         return Error.Failure(ex.Message);
    //     }
    // }
    //
    // public async Task<ErrorOr<Unit>> UnblockUserAsync(string userId)
    // {
    //     try
    //     {
    //         var userToUnBlock = await _userManager.FindByIdAsync(userId.ToString());
    //
    //         if (userToUnBlock == null)
    //         {
    //             return Error.Failure("User not found");
    //         }
    //
    //         userToUnBlock.IsBlocked = false;
    //
    //         var result = await _userManager.UpdateAsync(userToUnBlock);
    //
    //         if (!result.Succeeded)
    //         {
    //             return Error.Failure("Failed to block user");
    //         }
    //         
    //         return Unit.Value;
    //     }
    //     catch (Exception ex)
    //     {
    //         return Error.Failure(ex.Message);
    //     }   
    // }

    public async Task<ErrorOr<Unit>> UpdateUserAsync(UserEntity userEntity)
    {
        try
        {
            var existingUser = await _userManager.FindByIdAsync(userEntity.Id.ToString());
            if (existingUser == null)
            {
                return Error.Failure("User not found");
            }

            existingUser.FirstName = userEntity.FirstName;
            existingUser.LastName = userEntity.LastName;
            existingUser.Email = userEntity.Email;
            existingUser.Birthday = userEntity.Birthday;
            existingUser.Gender = userEntity.Gender;

            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                return Error.Failure("Failed to update user");
            }

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<List<UserEntity>>> SearchUsersByFirstNameAndLastNameAsync(string? firstName, string? lastName)
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
    
    public async Task<ErrorOr<UserEntity>> SaveUserAsync(UserEntity userEntity)
    {
        var saveResult = await _userManager.UpdateAsync(userEntity);

        if (!saveResult.Succeeded)
            return Error.Unexpected("Error saving user");

        return userEntity;
    }

    public async Task<List<UserEntity>> GetFriendsAsync(string userId)
    {
        // var user = await _userManager.FindByIdAsync(userId);
        // if (user == null)
        // {
        //     throw new ArgumentException("User not found", nameof(userId));
        // }
        //
        // var friends = user.Friends;
        //
        // return friends;
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Unit>> SendFriendRequestAsync(string userId, string friendId)
    {
        // var user = await _userManager.FindByIdAsync(userId);
        // if (user == null)
        // {
        //     return Error.Failure("User not found");
        // }
        //
        // var friend = await _userManager.FindByIdAsync(friendId);
        // if (friend == null)
        // {
        //     return Error.Failure("Friend not found");
        // }
        //
        // var result = user.SendFriendRequest(friend);
        //
        // if (!result)
        // {
        //     return Error.Failure("Failed to send friend request");
        // }
        //
        // var saveResult = await _userManager.UpdateAsync(user);
        //
        // if (!saveResult.Succeeded)
        // {
        //     return Error.Unexpected("Error saving user");
        // }
        //
        // return Unit.Value;
        throw new NotImplementedException();

    }

    public Task<ErrorOr<Unit>> AcceptFriendRequestAsync(string userId, string friendRequestId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> RejectFriendRequestAsync(string userId, string friendRequestId)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserEntity>> GetBlockedUsersAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> BlockUserByUserAsync(string userId, string blockedUserId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> UnblockUserByUserAsync(string userId, string blockedUserId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Unit>> SetProfilePrivacyAsync(string userId, bool isProfilePublic)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<bool>> GetProfilePrivacyAsync(string userId)
    {
        throw new NotImplementedException();
    }
}