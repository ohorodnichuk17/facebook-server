using ErrorOr;
using Facebook.Domain.User;

namespace Facebook.Application.Common.Interfaces.Authentication;

public interface IUserAuthenticationService
{
    Task<ErrorOr<string>> LoginUserAsync(UserEntity user, string password);
    Task<ErrorOr<Success>> LogoutUserAsync(string userId);
    Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user);
    Task<ErrorOr<Success>> ConfirmEmailAsync(Guid userId, string token);
    Task<bool> ResendEmailConfirmationAsync(UserEntity user, string emailToken, string baseUrl);
    Task<string> GeneratePasswordResetTokenAsync(UserEntity user);
    Task<ErrorOr<Success>> ResetPasswordAsync(UserEntity user, string token, string password);
    Task<string> GenerateEmailChangeTokenAsync(UserEntity user, string email);
    Task<ErrorOr<UserEntity>> ChangeEmailAsync(UserEntity user, string email, string token);
    Task<ErrorOr<UserEntity>> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword);

}
