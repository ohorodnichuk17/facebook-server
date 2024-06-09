using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Common.Interfaces.Admin.IRepository;
// ToDo ДОДАТИ МОЖОЛИВІСТЬ ЗАБОРОНЯТИ ЮЗЕРУ ДІЇ
public interface IAdminRepository : IUserRepository
{
    Task<ErrorOr<UserEntity>> CreateAsync(UserEntity user, string password, string role);
    new Task<ErrorOr<Unit>> DeleteUserAsync(string userId);
    Task<ErrorOr<UserEntity>> GetUserByEmailAsync(string email);
    new Task<ErrorOr<UserEntity>> GetUserByIdAsync(string userId);
    new Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync();
}