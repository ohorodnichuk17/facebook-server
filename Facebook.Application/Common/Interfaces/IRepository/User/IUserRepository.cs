using ErrorOr;
using Facebook.Application.DTO_s;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository.User;

public interface IUserRepository
{
    Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync();
    Task<ErrorOr<Unit>> UpdateAsync(UserEntity userEntity);

    Task<ErrorOr<UserEntity>> GetUserByIdAsync(Guid userId);
    Task<ErrorOr<UserEntity>> GetUserByIdAsync(string userId);
    Task<ErrorOr<UserEntity>> SaveUserAsync(UserEntity user);
    Task<ErrorOr<List<UserDto>>> SearchUsersByFirstNameAndLastNameAsync(string firstName, string lastName);
    Task<ErrorOr<UserEntity>> GetByEmailAsync(string email);
    Task<List<UserEntity>> GetAllFriendsAsync(string userId);
    Task<ErrorOr<List<UserEntity>>> GetAllFriendRequestsAsync(string userId);
    Task<ErrorOr<Unit>> SendFriendRequestAsync(string userId, string friendId);
    Task<ErrorOr<Unit>> AcceptFriendRequestAsync(string userId, string friendId);
    Task<ErrorOr<Unit>> RejectFriendRequestAsync(string userId, string friendRequestId);
    Task<ErrorOr<Unit>> RemoveFriendAsync(string userId, string friendId);
    Task<ErrorOr<UserEntity>> GetFriendByIdAsync(string userId, string friendId);
    Task<ErrorOr<FriendRequestEntity>> GetFriendRequestByFriendIdsAsync(string userId, string friendId);
    Task<ErrorOr<UserEntity>> GetFriendByIdAsync(Guid userId, Guid friendId);
    Task<ErrorOr<List<string>>> FindRolesByUserIdAsync(UserEntity user);
}