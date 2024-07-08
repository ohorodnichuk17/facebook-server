using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.ForgotPassword;
using Facebook.Application.Authentication.Login;
using Facebook.Application.Authentication.Register;
using Facebook.Application.Authentication.ResendConfirmEmail;
using Facebook.Application.Authentication.ResetPassword;
using Facebook.Contracts.Authentication.ChangeEmail;
using Facebook.Contracts.Authentication.Common.Response;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Facebook.Contracts.Authentication.ResendConfirmEmail;
using Facebook.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
public class AuthenticationController(ISender mediatr, IMapper mapper, IConfiguration configuration)
    : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm]RegisterRequest request)
    {
        var baseUrl = configuration.GetRequiredSection("HostSettings:ClientURL").Value;
        
        byte[] image = null;
        if (request.Avatar != null && request.Avatar.Length > 0)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await request.Avatar.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }

        var authResult = await mediatr.Send(mapper
            .Map<RegisterCommand>((request, baseUrl, image)));
        
        return authResult.Match(
            authResult => Ok(mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery]ConfirmEmailRequest request)
    {
        var confirmEmailResult = await mediatr.Send(mapper.Map<ConfirmEmailCommand>(request));

        return confirmEmailResult.Match(
            confirmResult => Redirect("http://localhost:5173/email-confirmed"),
            errors => Problem(errors));
    }

   
    // [HttpGet("resend-confirmation-email")]
    // public async Task<IActionResult> ResendConfirmationEmailAsync([FromQuery]ConfirmEmailRequest request)
    // {
    //     var baseUrl = configuration.GetRequiredSection("HostSettings:ClientURL").Value;
    //     var resendConfirmationResult = await mediatr.Send(mapper
    //         .Map<ResendConfirmEmailCommand>((request, baseUrl)));
    //
    //     return resendConfirmationResult.Match(
    //         success => Ok("Confirmation email resent successfully"),
    //         errors => Problem(errors));
    // }
    
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
    public async Task<IActionResult> ForgotPasswordAsync([FromQuery]String email)
    {
        await Console.Out.WriteLineAsync(email);

        var baseUrl = configuration.GetRequiredSection(
            "HostSettings:ClientURL").Value;

        await Console.Out.WriteLineAsync(baseUrl);

        var query = new ForgotPasswordQuery(email, baseUrl);

        await Console.Out.WriteLineAsync(query.Email);

        var forgotPasswordResult = await mediatr.Send(query);

        await Console.Out.WriteLineAsync(forgotPasswordResult.ToString());

        return forgotPasswordResult.Match(
            forgotPasswordRes => Redirect("http://localhost:5173/set-new-password"),
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
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return Ok("Logged out successfully");
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(DateTime.Now);
    }
}