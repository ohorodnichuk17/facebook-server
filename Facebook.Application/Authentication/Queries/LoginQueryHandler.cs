using Facebook.Application.Common.Interfaces.Authentication;
using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;

namespace Facebook.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<string>>
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

    public async Task<ErrorOr<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // var loginResult = await _authenticationService.LoginUserAsync(request.Email, request.Password);
        //
        // return loginResult;
        
        var errorOrUser = await _userRepository.GetByEmailAsync(request.Email);

        if (errorOrUser.IsError)
            return Error.Validation("User with this email doesn't exist");

        var user = errorOrUser.Value;

        //Login
        var errorOrLoginResult = await _authenticationService.LoginUserAsync(user, request.Password);

        if(errorOrLoginResult.IsError)
            return errorOrLoginResult;

        var role = errorOrLoginResult.Value;

        //Generate token
        var token = await _jwtTokenGenerator.GenerateJwtTokenAsync(user, role);

        return token;
    }
}