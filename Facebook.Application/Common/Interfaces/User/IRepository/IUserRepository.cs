using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Common.Interfaces.User.IRepository;

public interface IUserRepository
{
   Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync();
   Task<ErrorOr<UserEntity>> GetUserByIdAsync(string userId);
   Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity user, string password);
   Task<ErrorOr<Unit>> DeleteUserAsync(string userId);
   Task<ErrorOr<UserEntity>> SaveUserAsync(UserEntity user);
   Task<ErrorOr<List<UserEntity>>> SearchUsersByFirstNameAndLastNameAsync(string firstName, string lastName);
   Task<ErrorOr<UserEntity>> GetByEmailAsync(string email);
   Task<List<UserEntity>> GetAllFriendsAsync(string userId);
   Task<ErrorOr<Unit>> SendFriendRequestAsync(string userId, string friendId);
   Task<ErrorOr<Unit>> AcceptFriendRequestAsync(string userId, string friendRequestId);
   Task<ErrorOr<Unit>> RejectFriendRequestAsync(string userId, string friendRequestId);
   Task<ErrorOr<Unit>> RemoveFriendAsync(string userId, string friendId);
   Task<ErrorOr<UserEntity>> GetFriendByIdAsync(string userId, string friendId);
   Task<ErrorOr<List<string>>> FindRolesByUserIdAsync(UserEntity user);
}