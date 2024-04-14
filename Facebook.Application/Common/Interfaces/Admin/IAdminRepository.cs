using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.UserEntity;

namespace Facebook.Application.Common.Admin;

public interface IAdminRepository : IUserRepository
{
    Task<ErrorOr<UserEntity>> CreateAsync(UserEntity user, string password, string role);
    Task<ErrorOr<Unit>> BlockUserAsync(string userId);
    Task<ErrorOr<Unit>> UnblockUserAsync(string userId);
    Task<ErrorOr<Unit>> SetProfilePrivacyAsync(string userId, bool isProfilePublic);
    Task<ErrorOr<bool>> GetProfilePrivacyAsync(string userId);
    Task<ErrorOr<Unit>> DeleteUserAsync(string userId);
    Task<ErrorOr<UserEntity>> GetUserByEmailAsync(string email);
    Task<ErrorOr<UserEntity>> GetUserByIdAsync(string userId);
    Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync();
    Task<ErrorOr<List<UserEntity>>> SearchUsersAsync(string firstName, string lastName);
    Task<ErrorOr<List<UserEntity>>> GetRecentlyAddedUsersAsync(DateTime sinceDate);
    Task<ErrorOr<List<UserEntity>>> GetBlockedUsersAsync();
}