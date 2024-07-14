using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Application.Services;
using Microsoft.Extensions.Configuration;

namespace Facebook.Application.Authentication.ResendConfirmEmail;

public class ResendConfirmEmailCommandHandler(
    IUserAuthenticationService userAuthenticationService,
    IJwtGenerator jwtGenerator,
    IUserRepository userRepository,
    EmailService emailService,
    IConfiguration configuration)
    : IRequestHandler<ResendConfirmEmailCommand, ErrorOr<string>>
{
    private IJwtGenerator _jwtGenerator = jwtGenerator;
    private readonly EmailService _emailService = emailService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<ErrorOr<string>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userOrError = await userRepository.GetByEmailAsync(request.Email);
        if (userOrError.IsError)
        {
            return userOrError.Errors;
        }

        var user = userOrError.Value;
        var emailToken = await userAuthenticationService.GenerateEmailConfirmationTokenAsync(user);

        var emailResult = await userAuthenticationService.ResendEmailConfirmationAsync(user, emailToken, request.BaseUrl);
        if (!emailResult)
        {
            return Error.Failure("Failed to resend confirmation email.");
        }

        return emailToken;
    }
}
