using ErrorOr;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.User;

public class UserRepository(UserManager<UserEntity> userManager) : IUserRepository
{
    public async Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync()
    {
        try
        {
            var users = await userManager.Users.ToListAsync();
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

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Error.NotFound();
        }

        return user;
    }
    
    public async Task<ErrorOr<UserEntity>> GetByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Error.Failure("User not found");
        }

        return user;
    }

    public async Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity userEntity, string password)
    {
        userEntity.UserName = $"{userEntity.Email}".ToLower();

        var createUserResult = await userManager.CreateAsync(userEntity, password);

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

        var userToDelete = await userManager.FindByIdAsync(userId.ToString());
        if (userToDelete == null)
        {
            return Error.Failure("User not found");
        }

        var result = await userManager.DeleteAsync(userToDelete);
        if (!result.Succeeded)
        {
            return Error.Failure("Failed to delete user");
        }

        return Unit.Value;
    }

    public async Task<ErrorOr<List<string>>> FindRolesByUserIdAsync(UserEntity userEntity)
    {
        var roles = await userManager.GetRolesAsync(userEntity);
        if (roles == null)
        {
            return Error.NotFound();
        }

        return roles.ToList();
    }

    public async Task<ErrorOr<List<UserEntity>>> SearchUsersByFirstNameAndLastNameAsync(string? firstName, string? lastName)
    {
        try
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return Error.Failure("No search criteria provided");
            }

            var query = userManager.Users.AsQueryable();

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
        var saveResult = await userManager.UpdateAsync(userEntity);

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
}