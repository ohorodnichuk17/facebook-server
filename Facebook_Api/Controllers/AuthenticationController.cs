using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.ForgotPassword;
using Facebook.Application.Authentication.Login;
using Facebook.Application.Authentication.Register;
using Facebook.Application.Authentication.ResendConfirmEmail;
using Facebook.Application.Authentication.ResetPassword;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Contracts.Authentication.ChangeEmail;
using Facebook.Contracts.Authentication.Common.Response;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Facebook.Contracts.Authentication.ResendConfirmEmail;
using Facebook.Domain.Common.Errors;
using Facebook.Domain.User;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Facebook.Contracts.Authentication.Login.LoginRequest;
using RegisterRequest = Facebook.Contracts.Authentication.Register.RegisterRequest;
using ResetPasswordRequest = Facebook.Contracts.Authentication.ResetPassword.ResetPasswordRequest;

namespace Facebook.Server.Controllers;

[Route("api/authentication")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController(ISender mediatr,
    IMapper mapper,
    IConfiguration configuration,
    IUserAuthenticationService authenticationService,
    UserManager<UserEntity> userManager,
    ICurrentUserService currentUserService)
    : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest request)
    {
        var baseUrl = configuration.GetRequiredSection("HostSettings:ClientURL").Value;

        byte[] image = [];
        if (request.Avatar != null && request.Avatar.Length > 0)
        {
            using MemoryStream memoryStream = new();
            await request.Avatar.CopyToAsync(memoryStream);
            image = memoryStream.ToArray();
        }

        var authResult = await mediatr.Send(mapper
            .Map<RegisterCommand>((request, baseUrl, image)));

        return authResult.Match(
            authResult => Ok(mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailRequest request)
    {
        var confirmEmailResult = await mediatr.Send(mapper.Map<ConfirmEmailCommand>(request));

        return confirmEmailResult.Match(
            confirmResult => Redirect("https://qubix.itstep.click/email-confirmed"),
            errors => Problem(errors));
    }

    [HttpGet("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmailAsync([FromQuery] ResendConfirmEmailRequest request)
    {
        var baseUrl = configuration.GetRequiredSection("HostSettings:ClientURL").Value;
        var resendConfirmationResult = await mediatr.Send(mapper
            .Map<ResendConfirmEmailCommand>((request, baseUrl)));

        return resendConfirmationResult.Match(
            success => Ok("Confirmation email resent successfully"),
            errors => Problem(errors));
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var query = mapper.Map<LoginQuery>(request);
        var authenticationResult = await mediatr.Send(query);

        if (authenticationResult.IsError && authenticationResult
                .FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized,
                title: authenticationResult.FirstError.Description);
        }

        return authenticationResult.Match(
            authenticationResult => Ok(mapper
                .Map<AuthenticationResponse>(authenticationResult)),
            errors => Problem(errors));
    }

    [HttpGet("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromQuery] string email)
    {
        var baseUrl = Request.Headers["Referer"].ToString();

        var query = new ForgotPasswordQuery(email, baseUrl);

        var forgotPasswordResult = await mediatr.Send(query);

        return forgotPasswordResult.Match(
            forgotPasswordRes => Redirect("https://qubix.itstep.click/set-new-password"),
            errors => Problem(errors));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        Console.WriteLine($"Спроба скинути пароль для користувача з email: {request.Email}");

        var baseUrl = configuration.GetRequiredSection("HostSettings:ClientURL").Value;

        var resetPasswordCommand = mapper.Map<ResetPasswordCommand>(request);
        resetPasswordCommand = resetPasswordCommand with { BaseUrl = baseUrl };

        var resetPasswordResult = await mediatr.Send(resetPasswordCommand);

        return resetPasswordResult.Match(
            resetPasswordRes =>
            {
                Console.WriteLine($"Пароль для користувача {request.Email} успішно скинуто.");
                return Ok(resetPasswordResult.Value);
            },
            errors =>
            {
                Console.WriteLine($"Помилка при скиданні пароля для користувача {request.Email}.");
                return Problem(errors);
            }
        );
    }


    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
    {
        var baseUrl = configuration.GetRequiredSection("HostSettings:ClientURL").Value;
        var changeEmailCommand = mapper.Map<ChangeEmailCommand>(request);
        changeEmailCommand = changeEmailCommand with { BaseUrl = baseUrl };
        var changeEmailResult = await mediatr.Send(changeEmailCommand);

        return changeEmailResult.Match(
            changeEmailRes => Ok(changeEmailResult.Value),
            errors => Problem(errors));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        var logoutResult = await authenticationService.LogoutUserAsync(currentUserId);

        if (logoutResult.IsError)
        {
            return Problem(logoutResult.Errors);
        }

        return Ok("Logged out successfully");
    }

    [HttpGet("user-status/{userId}")]
    public async Task<IActionResult> GetUserStatusAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var status = new
        {
            IsOnline = user.IsOnline,
            LastActive = user.LastActive
        };

        return Ok(status);
    }


    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(DateTime.Now);
    }
}