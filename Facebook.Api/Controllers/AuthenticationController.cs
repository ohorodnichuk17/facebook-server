using Facebook.Application.Authentication.Commands.Register;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.Queries;
using Facebook.Contracts.Authentication.Common;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Facebook.Contracts.Authentication.Login;
using Facebook.Contracts.Authentication.Register;
using Facebook.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailRequest request)
    {
        var confirmEmailResult = await _mediatr.Send(_mapper.Map<ConfirmEmailCommand>(request));

        return confirmEmailResult.Match(
            authResult => Ok(confirmEmailResult),
            errors => Problem(errors[0].ToString()));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        // var loginResult = await _mediatr.Send(_mapper.Map<LoginQuery>(request));
        //
        // return loginResult.Match(
        //     loginResult => Ok(loginResult),
        //     errors => Problem(errors));

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
}