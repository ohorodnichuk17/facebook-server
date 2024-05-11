using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Authentication.Commands.Register;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.ForgotPassword;
using Facebook.Application.Authentication.Queries;
using Facebook.Application.Authentication.ResetPassword;
using Facebook.Contracts.Authentication.ChangeEmail;
using Facebook.Contracts.Authentication.Common;
using Facebook.Contracts.Authentication.Common.Response;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Facebook.Contracts.Authentication.ForgotPassword;
using Facebook.Contracts.Authentication.Login;
using Facebook.Contracts.Authentication.Register;
using Facebook.Contracts.Authentication.ResetPassword;
using Facebook.Domain.Common.Errors;
using Facebook.Server.Infrastructure.NLog;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediatr;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    
    public AuthenticationController(ISender mediatr, IMapper mapper, IConfiguration configuration)
    {
        _mediatr = mediatr;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm]RegisterRequest request)
    {
        var baseUrl = _configuration.GetRequiredSection("HostSettings:ClientURL").Value;
        
        byte[] image = null;
        if (request.Avatar != null && request.Avatar.Length > 0)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await request.Avatar.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }
        
        var authResult = await _mediatr.Send(_mapper
            .Map<RegisterCommand>((request, baseUrl, image)));
        
        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }


    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery]ConfirmEmailRequest request)
    {
        var confirmEmailResult = await _mediatr.Send(_mapper.Map<ConfirmEmailCommand>(request));

        return confirmEmailResult.Match(
            authResult => Ok(confirmEmailResult.Value),
            errors => Problem(errors));
      
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var authenticationResult = await _mediatr.Send(query);

        if (authenticationResult.IsError && authenticationResult
                .FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized,
                title: authenticationResult.FirstError.Description);
        }

        return authenticationResult.Match(
            authenticationResult => Ok(_mapper
                .Map<AuthenticationResponse>(authenticationResult)),
            errors => Problem(errors));
    }
    
    [HttpGet("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromQuery]String email)
    {
        await Console.Out.WriteLineAsync(email);

        var baseUrl = _configuration.GetRequiredSection(
            "HostSettings:ClientURL").Value;

        await Console.Out.WriteLineAsync(baseUrl);

        var query = new ForgotPasswordQuery(email, baseUrl);

        await Console.Out.WriteLineAsync(query.Email);

        var forgotPasswordResult = await _mediatr.Send(query);

        await Console.Out.WriteLineAsync(forgotPasswordResult.ToString());

        return forgotPasswordResult.Match(
            forgotPasswordRes => Ok(forgotPasswordResult.Value),
            errors => Problem(errors));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        // Логування перед викликом методу ResetPasswordAsync
        Console.WriteLine($"Спроба скинути пароль для користувача з email: {request.Email}");
    
        var baseUrl = _configuration.GetRequiredSection("HostSettings:ClientURL").Value;

        var resetPasswordCommand = _mapper.Map<ResetPasswordCommand>(request); 
        resetPasswordCommand = resetPasswordCommand with { BaseUrl = baseUrl };
    
        var resetPasswordResult = await _mediatr.Send(resetPasswordCommand);
    
        // Логування після виклику методу ResetPasswordAsync
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
        var baseUrl = _configuration.GetRequiredSection("HostSettings:ClientURL").Value;
        var changeEmailCommand = _mapper.Map<ChangeEmailCommand>(request); 
        changeEmailCommand = changeEmailCommand with { BaseUrl = baseUrl }; 
        var changeEmailResult = await _mediatr.Send(changeEmailCommand);
  
        return changeEmailResult.Match(
            changeEmailRes => Ok(changeEmailResult.Value),
            errors => Problem(errors));
    }


    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(DateTime.Now);
    }
}