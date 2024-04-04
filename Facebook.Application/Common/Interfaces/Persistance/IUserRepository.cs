using Facebook.Domain.User;
using ErrorOr;
using MediatR;

namespace Facebook.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
   // For admin
   Task<ErrorOr<List<User>>> GetAllUsersAsync();
   Task<ErrorOr<User>> GetUserByIdAsync(Guid userId);
   Task<ErrorOr<User>> CreateUserAsync(User user, string password, string role);
   Task<ErrorOr<Unit>> DeleteUserAsync(Guid userId);
   
   // For admin & user
   Task<ErrorOr<Unit>> UpdateUserAsync(User user, string? newPassword);


   // For user
   Task<ErrorOr<List<User>>> SearchUsersAsync(string firstName, string lastName);
   Task<ErrorOr<User>> GetByEmailAsync(string email);

   Task<List<User>> GetFriendsAsync(Guid userId);
   Task<ErrorOr<Unit>> SendFriendRequestAsync(Guid userId, Guid friendId);
   Task<ErrorOr<Unit>> AcceptFriendRequestAsync(Guid userId, Guid friendRequestId);
   Task<ErrorOr<Unit>> RejectFriendRequestAsync(Guid userId, Guid friendRequestId);
   Task<List<User>> GetBlockedUsersAsync(Guid userId);
   Task<ErrorOr<Unit>> SetProfilePrivacyAsync(Guid userId, bool isProfilePublic);
   Task<ErrorOr<bool>> GetProfilePrivacyAsync(Guid userId);
   Task<ErrorOr<Unit>> BlockUserAsync(Guid userId);
   Task<ErrorOr<Unit>> UnblockUserAsync(Guid userId);
}