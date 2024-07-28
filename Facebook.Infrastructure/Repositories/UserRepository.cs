using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Application.DTO_s;
using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.User;

public class UserRepository(UserManager<UserEntity> userManager, FacebookDbContext context) : IUserRepository
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

    public async Task<ErrorOr<Unit>> UpdateAsync(UserEntity userEntity)
    {
        var updateResult = await userManager.UpdateAsync(userEntity);

        if (!updateResult.Succeeded)
            return Error.Unexpected("Error updating user");

        return Unit.Value;
    }

    public async Task<ErrorOr<UserEntity>> GetUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId), "userId cannot be null");
        }

        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return Error.NotFound();
        }

        return user;
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

    public async Task<ErrorOr<List<UserDto>>> SearchUsersByFirstNameAndLastNameAsync(string firstName, string lastName)
    {
        try
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return Error.Failure("No search criteria provided");
            }

            var usersQuery = userManager.Users
               .Include(u => u.Stories)
               .Include(u => u.Posts)
               .AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                usersQuery = usersQuery.Where(u => u.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                usersQuery = usersQuery.Where(u => u.LastName.Contains(lastName));
            }

            var users = await usersQuery.ToListAsync();

            var userIds = users.Select(u => u.Id).ToList();

            var profilesQuery = context.UsersProfiles
               .Where(up => userIds.Contains(up.UserId))
               .ToDictionaryAsync(up => up.UserId);

            var profiles = await profilesQuery;

            var result = users.Select(u => new UserDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Avatar = u.Avatar,
                IsProfilePublic = profiles.ContainsKey(u.Id) && profiles[u.Id].IsProfilePublic,
                Birthday = profiles.ContainsKey(u.Id) && profiles[u.Id].IsProfilePublic ? u.Birthday : null,
                Gender = profiles.ContainsKey(u.Id) && profiles[u.Id].IsProfilePublic ? u.Gender : null,
                Stories = profiles.ContainsKey(u.Id) && profiles[u.Id].IsProfilePublic
                  ? u.Stories.ToList()
                  : new List<StoryEntity>(),
                Posts = profiles.ContainsKey(u.Id) && profiles[u.Id].IsProfilePublic
                  ? u.Posts.ToList()
                  : new List<PostEntity>()
            }).ToList();

            if (result.Count == 0)
            {
                return Error.Failure("No users found");
            }

            return result;
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

    public async Task<List<UserEntity>> GetAllFriendsAsync(string userId)
    {
        var users = await context.FriendRequests
           .Include(u => u.Sender)
           .Include(u => u.Receiver)
           .Where(u => u.ReceiverId.ToString() == userId || u.SenderId.ToString() == userId)
           .Select(u => u.ReceiverId.ToString() == userId ? u.Sender : u.Receiver)
           .ToListAsync();

        if (users.Count == 0)
        {
            throw new Exception("Friends not found");
        }

        return users;
    }


    public async Task<ErrorOr<Unit>> SendFriendRequestAsync(string userId, string friendId)
    {
        var sender = await userManager.Users
           .Include(u => u.SentFriendRequests)
           .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (sender == null)
        {
            return Error.Failure("Sender not found");
        }

        var receiver = await userManager.Users
           .Include(u => u.ReceivedFriendRequests)
           .FirstOrDefaultAsync(u => u.Id.ToString() == friendId);

        if (receiver == null)
        {
            return Error.Failure("Receiver not found");
        }

        var friendRequest = new FriendRequestEntity
        {
            Id = Guid.NewGuid(),
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            SentAt = DateTime.UtcNow,
            IsAccepted = false
        };

        sender.SentFriendRequests.Add(friendRequest);
        receiver.ReceivedFriendRequests.Add(friendRequest);

        await context.FriendRequests.AddAsync(friendRequest);
        await context.SaveChangesAsync();

        return Unit.Value;
    }

    public async Task<ErrorOr<Unit>> AcceptFriendRequestAsync(string userId, string friendId)
    {
        try
        {
            var friendRequest = await context.FriendRequests
               .FirstOrDefaultAsync(fr => fr.SenderId.ToString() == friendId && fr.ReceiverId.ToString() == userId);

            if (friendRequest == null)
            {
                return Error.Failure("Friend request not found");
            }

            if (friendRequest.IsAccepted)
            {
                return Error.Failure("Friend request already accepted");
            }

            friendRequest.IsAccepted = true;
            context.FriendRequests.Update(friendRequest);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.ToString());
        }
    }

    public async Task<ErrorOr<Unit>> RejectFriendRequestAsync(string userId, string friendRequestId)
    {
        try
        {
            var friendRequest = await context.FriendRequests
               .FirstOrDefaultAsync(fr => fr.Id.ToString() == friendRequestId && fr.ReceiverId.ToString() == userId);

            if (friendRequest == null)
            {
                return Error.Failure("Friend request not found");
            }

            context.FriendRequests.Remove(friendRequest);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.ToString());
        }
    }

    public async Task<ErrorOr<Unit>> RemoveFriendAsync(string userId, string friendId)
    {
        try
        {
            var friendRequest = await context.FriendRequests
               .FirstOrDefaultAsync(fr =>
                  ((fr.SenderId.ToString() == userId && fr.ReceiverId.ToString() == friendId) ||
                   (fr.SenderId.ToString() == friendId && fr.ReceiverId.ToString() == userId)) &&
                  fr.IsAccepted);

            if (friendRequest == null)
            {
                return Error.Failure("Friend relationship not found");
            }

            context.FriendRequests.Remove(friendRequest);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }



    public async Task<ErrorOr<UserEntity>> GetFriendByIdAsync(string userId, string friendId)
    {
        try
        {
            var friendRequest = await context.FriendRequests
                .Include(fr => fr.Sender)
                .Include(fr => fr.Receiver)
                .FirstOrDefaultAsync(fr =>
                    (fr.SenderId.ToString() == userId && fr.ReceiverId.ToString() == friendId) ||
                    (fr.SenderId.ToString() == friendId && fr.ReceiverId.ToString() == userId) &&
                    fr.IsAccepted);

            if (friendRequest == null)
            {
                Console.Error.WriteLine("Friend not found");
                return Error.Failure("Friend not found");
            }

            var friend = friendRequest.SenderId.ToString() == userId ? friendRequest.Receiver : friendRequest.Sender;

            return friend;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving friend: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<List<UserEntity>>> GetAllFriendRequestsAsync(string userId)
    {
        try
        {
            var friendRequests = await context.FriendRequests
                .Where(fr => fr.ReceiverId.ToString() == userId && !fr.IsAccepted)
                .ToListAsync();

            if (friendRequests is null)
            {
                return Error.Failure("Friend request not found");
            }

            var senderIds = friendRequests.Select(fr => fr.SenderId).ToList();

            var users = await context.Users
                .Where(user => senderIds.Contains(user.Id))
                .ToListAsync();

            return users;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.ToString());
        }
    }
}