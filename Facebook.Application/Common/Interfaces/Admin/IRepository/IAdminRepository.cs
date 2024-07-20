using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Common.Interfaces.Admin.IRepository;
// ToDo ДОДАТИ МОЖОЛИВІСТЬ ЗАБОРОНЯТИ ЮЗЕРУ ДІЇ
public interface IAdminRepository : IUserRepository
{
    Task<ErrorOr<UserEntity>> CreateAsync(UserEntity user, string password, string role);
    Task<ErrorOr<UserEntity>> GetUserByEmailAsync(string email);
    Task<ErrorOr<Unit>> BlockUserAsync(string userId);
    Task<ErrorOr<Unit>> UnblockUserAsync(string userId);
    Task<ErrorOr<Unit>> BanUserAsync(string userId);
    Task<ErrorOr<Unit>> UnbanUserAsync(string userId);
    Task<ErrorOr<Unit>> RemovePostAsync(string postId);
    Task<ErrorOr<Unit>> RemoveCommentAsync(string commentId);
    Task<ErrorOr<Unit>> RemoveStoryAsync(string storyId);
}