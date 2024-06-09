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
   Task<List<UserEntity>> GetFriendsAsync(string userId);
   Task<ErrorOr<List<string>>> FindRolesByUserIdAsync(UserEntity user);
}