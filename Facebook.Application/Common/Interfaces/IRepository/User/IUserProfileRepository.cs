using ErrorOr;
using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Common.Interfaces.IRepository.User;

public interface IUserProfileRepository : IRepository<UserProfileEntity>
{
    Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile,
        string firstName, string lastName, string avatar, bool isOnline);
    Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile);
    Task<ErrorOr<UserProfileEntity>> UserCreateProfileAsync(Guid userId);
    Task<ErrorOr<UserProfileEntity>> GetUserProfileByIdAsync(string userId);
    Task<ErrorOr<bool>> DeleteUserProfileAsync(string userId);
    Task<ErrorOr<IEnumerable<PostEntity>>> GetPostsByUserIdAsync(Guid userId);
    Task<ErrorOr<IEnumerable<StoryEntity>>> GetStoriesByUserIdAsync(Guid userId);
}
