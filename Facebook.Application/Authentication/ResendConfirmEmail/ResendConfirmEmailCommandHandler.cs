using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Application.Services;
using Microsoft.Extensions.Configuration;

namespace Facebook.Application.Authentication.ResendConfirmEmail;

public class ResendConfirmEmailCommandHandler : IRequestHandler<ResendConfirmEmailCommand, ErrorOr<string>>
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private IJwtGenerator _jwtGenerator;
    private readonly IUserRepository _userRepository;
    private readonly EmailService _emailService;
    private readonly IConfiguration _configuration;

    public ResendConfirmEmailCommandHandler(IUserAuthenticationService userAuthenticationService, IJwtGenerator jwtGenerator, IUserRepository userRepository, EmailService emailService, IConfiguration configuration)
    {
        _userAuthenticationService = userAuthenticationService;
        _jwtGenerator = jwtGenerator;
        _userRepository = userRepository;
        _emailService = emailService;
        _configuration = configuration;
    }
    
    public async Task<ErrorOr<string>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userOrError = await _userRepository.GetUserByIdAsync(request.UserId.ToString());
        if (userOrError.IsError)
        {
            return userOrError.Errors;
        }

        var user = userOrError.Value;
        var emailToken = await _userAuthenticationService.GenerateEmailConfirmationTokenAsync(user);

        var emailResult = await _userAuthenticationService.ResendEmailConfirmationAsync(user, emailToken, request.BaseUrl);
        if (!emailResult)
        {
            return Error.Failure("Failed to resend confirmation email.");
        }

        return emailToken;
    }
}
