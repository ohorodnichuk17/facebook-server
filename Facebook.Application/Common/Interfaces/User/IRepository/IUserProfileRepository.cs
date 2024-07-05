using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Common.Interfaces.User.IRepository;

public interface IUserProfileRepository : IRepository<UserProfileEntity>
{
    public Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile,
        string firstName, string lastName, string avatar);
    Task<ErrorOr<UserProfileEntity>> UserCreateProfileAsync(Guid userId);
    Task<ErrorOr<Unit>> BlockUserAsync(string userId);
    Task<ErrorOr<Unit>> UnblockUserAsync(string userId);
    Task<ErrorOr<UserProfileEntity>> GetUserProfileByIdAsync(string userId);

    Task<ErrorOr<bool>> DeleteUserProfileAsync(string userId);
}
