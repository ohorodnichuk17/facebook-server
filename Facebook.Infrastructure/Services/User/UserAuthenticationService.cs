using System.Net;
using System.Text;
using ErrorOr;
using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Facebook.Infrastructure.Services.User;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager; 
    private readonly ILogger<ChangeEmailCommandHandler> _logger; 

    public UserAuthenticationService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, ILogger<ChangeEmailCommandHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<ErrorOr<string>> LoginUserAsync(UserEntity user, string password)
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
    
    public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return token;
    }

    public async Task<ErrorOr<Success>> ConfirmEmailAsync(Guid userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return Error.NotFound();
        
        var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);

        if (!confirmEmailResult.Succeeded)
        {
            var errorDescriptions = confirmEmailResult.Errors.Select(e => e.Description).ToList();
            return Error.Failure($"User email is not confirmed! Reasons: {string.Join(", ", errorDescriptions)}");
        }

        return Result.Success;
    }
    
    public async Task<string> GeneratePasswordResetTokenAsync(UserEntity user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return token;
    }

    public async Task<ErrorOr<Success>> ResetPasswordAsync(UserEntity user, string token, string password)
    {
        Console.WriteLine($"Початковий пароль для користувача {user.UserName}: {password}");
        var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, password);

        if (resetPasswordResult.Succeeded)
        {
            Console.WriteLine($"Пароль для користувача {user.UserName} успішно змінено.");

            return Result.Success;
        }
        else
        {
            Console.WriteLine($"Помилка зміни пароля для користувача {user.UserName}: {resetPasswordResult.Errors.FirstOrDefault()?.Description}");
            return Error.Validation(resetPasswordResult.Errors.FirstOrDefault()?.Description.ToString());
        }
    }

    public async Task<string> GenerateEmailChangeTokenAsync(UserEntity user, string email)
    {
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, email);
        return token;
    }

    public async Task<ErrorOr<UserEntity>> ChangeEmailAsync(UserEntity user, string email, string token)
    {
        // var decodedToken = WebEncoders.Base64UrlDecode(token);
        // var normalToken = Encoding.UTF8.GetString(decodedToken);
        var changeEmailResult = await _userManager.ChangeEmailAsync(user, email, token);
    
        if (!changeEmailResult.Succeeded)
        {
            return Error.Validation(changeEmailResult.Errors.FirstOrDefault()!.Description.ToString());
        }
    
        return user;
    }

    public async Task<ErrorOr<UserEntity>> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
    {
        var changePassword = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!changePassword.Succeeded)
        {
            return Error.Validation(changePassword.Errors.FirstOrDefault()!.Description.ToString());
        }

        return user;
    }
}
