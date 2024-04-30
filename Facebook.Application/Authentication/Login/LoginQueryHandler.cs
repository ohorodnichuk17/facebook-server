using MediatR;
using Microsoft.Extensions.Logging;
using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.Common.Errors;

namespace Facebook.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly ILogger<LoginQueryHandler> _logger;

    public LoginQueryHandler(
        IUserAuthenticationService authenticationService, 
        IUserRepository userRepository, 
        IJwtGenerator jwtGenerator, 
        ILogger<LoginQueryHandler> logger)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _logger = logger;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to authenticate user {Email}", request.Email);

        var errorOrUser = await _userRepository.GetByEmailAsync(request.Email);
        
        if (errorOrUser.IsError)
        {
            _logger.LogWarning("User with email {Email} not found", request.Email);
            return Error.Validation("User with this email doesn't exist");
        }
        
        var user = errorOrUser.Value;
        
        var errorOrLoginResult = await _authenticationService.LoginUserAsync(user, request.Password);

        if (errorOrLoginResult.IsError)
        {
            _logger.LogError("Authentication failed for user {Email}", request.Email);
            return errorOrLoginResult.Errors;
        }

        var role = errorOrLoginResult.Value;
        var token = await _jwtGenerator.GenerateJwtTokenAsync(user, role);

        _logger.LogInformation("Authentication successful for user {Email}, role {Role}", request.Email, role);

        return new AuthenticationResult(user.Id, user, token);
    }
}
