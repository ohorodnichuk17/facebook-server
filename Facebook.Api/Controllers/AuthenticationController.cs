using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Authentication.Commands.Register;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.ForgotPassword;
using Facebook.Application.Authentication.Queries;
using Facebook.Application.Authentication.ResetPassword;
using Facebook.Contracts.Authentication.ChangeEmail;
using Facebook.Contracts.Authentication.Common;
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
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var baseUrl = _configuration.GetRequiredSection("HostSettings:ClientURL").Value;
        var authResult = await _mediatr.Send(_mapper.Map<RegisterCommand>((request, baseUrl)));
        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailRequest request)
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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
    {
        var baseUrl = _configuration.GetRequiredSection(
            "HostSettings:ClientURL").Value;
        var forgotPasswordResult = await _mediatr.Send(
            _mapper.Map<ForgotPasswordQuery>((request, baseUrl)));

        return forgotPasswordResult.Match(
            forgotPasswordResult => Ok(forgotPasswordResult),
            errors => Problem(errors));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        var resetPasswordResult = await _mediatr.Send(
            _mapper.Map<ResetPasswordCommand>(request));

        return resetPasswordResult.Match(
            resetPasswordResult => Ok(),
            errors => Problem(errors));
    }

    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
    {
        var changeEmailResult = await _mediatr.Send(
            _mapper.Map<ChangeEmailCommand>(request));

        return changeEmailResult.Match(
            changeEmailResult => Ok(), 
            errors => Problem(errors));
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(DateTime.Now);
    }
}