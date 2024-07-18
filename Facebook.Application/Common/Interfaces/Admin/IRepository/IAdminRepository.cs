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
    Task<ErrorOr<Unit>> BlockUserAsync(string userId);
    Task<ErrorOr<Unit>> UnblockUserAsync(string userId);
    Task<ErrorOr<Unit>> BanUserAsync(string userId);
    Task<ErrorOr<Unit>> UnbanUserAsync(string userId);
    Task<ErrorOr<Unit>> RemovePostAsync(string postId);
    Task<ErrorOr<Unit>> RemoveCommentAsync(string commentId);
    Task<ErrorOr<Unit>> RemoveStoryAsync(string storyId);
}