using ErrorOr;
using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Services;
using Facebook.Domain.TypeExtensions;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Facebook.Infrastructure.Services.User;

public class UserAuthenticationService(
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    ILogger<ChangeEmailCommandHandler> logger,
    EmailService emailService)
    : IUserAuthenticationService
{
    private readonly ILogger<ChangeEmailCommandHandler> _logger = logger;

    public async Task<ErrorOr<string>> LoginUserAsync(UserEntity user, string password)
    {
        var signinResult = await signInManager.PasswordSignInAsync(user, password,
            isPersistent: true, lockoutOnFailure: true);

        if (signinResult.IsNotAllowed)
            return Error.Forbidden("Email is not confirmed");

        if (signinResult.IsLockedOut)
            return Error.Forbidden("User is blocked");

        if (!signinResult.Succeeded)
            return Error.Failure("Wrong password");

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

        if (role == null)
            return Error.NotFound("Role of user is not found");

        user.IsOnline = true;
        user.LastActive = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        return role;
    }

    public async Task<ErrorOr<Success>> LogoutUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.IsOnline = false;
            user.LastActive = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
        }

        await signInManager.SignOutAsync();
        return Result.Success;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(UserEntity user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        return token;
    }

    public async Task<ErrorOr<Success>> ConfirmEmailAsync(Guid userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return Error.NotFound();

        var confirmEmailResult = await userManager.ConfirmEmailAsync(user, token);

        if (!confirmEmailResult.Succeeded)
        {
            var errorDescriptions = confirmEmailResult.Errors.Select(e => e.Description).ToList();
            return Error.Failure($"User email is not confirmed! Reasons: {string.Join(", ", errorDescriptions)}");
        }

        return Result.Success;
    }

    public async Task<bool> ResendEmailConfirmationAsync(UserEntity user, string emailToken, string baseUrl)
    {
        string? userName = !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)
            ? $"{user.FirstName} {user.LastName}"
            : user.Email;

        var emailResult = await emailService.SendEmailConfirmationEmailAsync(user.Id, user.Email!, emailToken, baseUrl, userName!);

        return emailResult.IsSuccess();
    }


    public async Task<string> GeneratePasswordResetTokenAsync(UserEntity user)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        return token;
    }

    public async Task<ErrorOr<Success>> ResetPasswordAsync(UserEntity user, string token, string password)
    {
        Console.WriteLine($"Початковий пароль для користувача {user.UserName}: {password}");
        var resetPasswordResult = await userManager.ResetPasswordAsync(user, token, password);

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
        var token = await userManager.GenerateChangeEmailTokenAsync(user, email);
        return token;
    }

    public async Task<ErrorOr<UserEntity>> ChangeEmailAsync(UserEntity user, string email, string token)
    {
        var changeEmailResult = await userManager.ChangeEmailAsync(user, email, token);

        if (!changeEmailResult.Succeeded)
        {
            return Error.Validation(changeEmailResult.Errors.FirstOrDefault()!.Description.ToString());
        }

        return user;
    }

    public async Task<ErrorOr<UserEntity>> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
    {
        var changePassword = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!changePassword.Succeeded)
        {
            return Error.Validation(changePassword.Errors.FirstOrDefault()!.Description.ToString());
        }

        return user;
    }
}
