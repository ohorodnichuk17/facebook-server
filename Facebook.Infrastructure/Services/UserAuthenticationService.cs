using System.Text;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Facebook.Infrastructure.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserAuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return token;
    }

    public async Task<ErrorOr<string>> LoginUserAsync(User user, string password)
    {
        var signinResult = await _signInManager.PasswordSignInAsync(user, password,
            isPersistent: true, lockoutOnFailure: true);
		
        if (signinResult.IsNotAllowed)
            return Error.Forbidden("Email is not confirmed");

        if (signinResult.IsLockedOut)
            return Error.Forbidden("User is blocked");

        if (!signinResult.Succeeded)
            return Error.Failure("Wrong password");

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (role == null)
            return Error.NotFound("Role of user is not found");

        return role;
    }

    public async Task<ErrorOr<Success>> LogoutUserAsync(Guid userId)
    {
        await _signInManager.SignOutAsync();
        return Result.Success;
    }

    public async Task<ErrorOr<Success>> ConfirmEmailAsync(Guid userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return Error.NotFound();

        var decoderToken = WebEncoders.Base64UrlDecode(token);

        var normalToken = Encoding.UTF8.GetString(decoderToken);

        var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, normalToken);

        if (!confirmEmailResult.Succeeded)
            return Error.Failure("User email is not confirmed!");

        return Result.Success;
    }
    
    public async Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return token;
    }

    public async Task<ErrorOr<User>> ResetPasswordAsync(User user, string token, string password)
    {
        var decodedToken = WebEncoders.Base64UrlDecode(token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);
        var result = await _userManager.ResetPasswordAsync(user, normalToken, password);

        if (result.Succeeded)
        {
            return user;
        }
        else
        {
            return Error.Validation(result.Errors.FirstOrDefault()!.Description.ToString());
        }
    }

    public async Task<string> GenerateEmailChangeTokenAsync(User user, string email)
    {
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, email);
        return token;
    }

    public async Task<ErrorOr<User>> ChangeEmailAsync(User user, string email, string token)
    {
        var decodedToken = WebEncoders.Base64UrlDecode(token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);
        var changeEmailResult = await _userManager.ChangeEmailAsync(user, email, normalToken);

        if (!changeEmailResult.Succeeded)
        {
            return Error.Validation(changeEmailResult.Errors.FirstOrDefault()!.Description.ToString());
        }

        return user;
    }

    public async Task<ErrorOr<User>> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var changePassword = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!changePassword.Succeeded)
        {
            return Error.Validation(changePassword.Errors.FirstOrDefault()!.Description.ToString());
        }

        return user;
    }
}
