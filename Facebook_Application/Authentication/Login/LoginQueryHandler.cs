using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Facebook.Application.Authentication.Login;

public class LoginQueryHandler(
    IUserAuthenticationService authenticationService,
    IUnitOfWork unitOfWork,
    IJwtGenerator jwtGenerator,
    ILogger<LoginQueryHandler> logger)
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to authenticate user {Email}", request.Email);

        var errorOrUser = await unitOfWork.User.GetByEmailAsync(request.Email);

        if (errorOrUser.IsError)
        {
            logger.LogWarning("User with email {Email} not found", request.Email);
            return Error.Validation("User with this email doesn't exist");
        }

        var user = errorOrUser.Value;

        var errorOrLoginResult = await authenticationService.LoginUserAsync(user, request.Password);

        if (errorOrLoginResult.IsError)
        {
            logger.LogError("Authentication failed for user {Email}", request.Email);
            return errorOrLoginResult.Errors;
        }

        var role = errorOrLoginResult.Value;
        var token = await jwtGenerator.GenerateJwtTokenAsync(user, role);

        logger.LogInformation("Authentication successful for user {Email}, role {Role}", request.Email, role);

        return new AuthenticationResult(user.Id, user, token);
    }
}
