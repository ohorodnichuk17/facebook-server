using Facebook.Application.Common.Interfaces.Authentication;
using MediatR;
using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.Common.Errors;
using Facebook.Domain.User;

namespace Facebook.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(IUserAuthenticationService authenticationService, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        var errorOrUser = await _userRepository.GetByEmailAsync(request.Email);
        
        if (errorOrUser.IsError)
            return Error.Validation("User with this email doesn't exist");
        
        var user = errorOrUser.Value;
        
        var errorOrLoginResult = await _authenticationService.LoginUserAsync(user, request.Password);
        
        if (user.PasswordHash != request.Password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var role = errorOrLoginResult.Value;


        var token = _jwtTokenGenerator.GenerateJwtTokenAsync(user, role);

        return new AuthenticationResult(user.Id, user, token);
    }
}