using Facebook.Domain.UserEntity;
using ErrorOr;
using MediatR;

namespace Facebook.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
   // TEMP
   Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity user, string password, string role);

   
   Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync();
   Task<ErrorOr<UserEntity>> GetUserByIdAsync(string userId);
   Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity user, string password);
   Task<ErrorOr<Unit>> DeleteUserAsync(string userId);
   Task<ErrorOr<UserEntity>> SaveUserAsync(UserEntity user);
   Task<ErrorOr<List<UserEntity>>> SearchUsersByFirstNameAndLastNameAsync(string firstName, string lastName);
   Task<ErrorOr<UserEntity>> GetByEmailAsync(string email);
   Task<List<UserEntity>> GetFriendsAsync(string userId);
   Task<List<UserEntity>> GetBlockedUsersAsync(string userId);
   Task<ErrorOr<Unit>> SetProfilePrivacyAsync(string userId, bool isProfilePublic);
   Task<ErrorOr<bool>> GetProfilePrivacyAsync(string userId);
   Task<ErrorOr<List<string>>> FindRolesByUserIdAsync(UserEntity user);
   // Task<ErrorOr<Unit>> BlockUserAsync(string userId);
   // Task<ErrorOr<Unit>> UnblockUserAsync(string userId);
   Task<ErrorOr<Unit>> UpdateUserAsync(UserEntity user);
}