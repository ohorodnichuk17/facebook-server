using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Services;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Facebook.Application.Authentication.ResendConfirmEmail;

public class ResendConfirmEmailCommandHandler(
    IUserAuthenticationService userAuthenticationService,
    IJwtGenerator jwtGenerator,
    IUnitOfWork unitOfWork,
    EmailService emailService,
    IConfiguration configuration)
    : IRequestHandler<ResendConfirmEmailCommand, ErrorOr<string>>
{
    private IJwtGenerator _jwtGenerator = jwtGenerator;
    private readonly EmailService _emailService = emailService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<ErrorOr<string>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userOrError = await unitOfWork.User.GetByEmailAsync(request.Email);
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
