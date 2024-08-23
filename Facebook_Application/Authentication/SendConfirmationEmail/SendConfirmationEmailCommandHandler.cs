using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Services;
using MediatR;

namespace Facebook.Application.Authentication.SendConfirmationEmail;

public class SendConfirmationEmailCommandHandler(
    IUnitOfWork unitOfWork,
    IUserAuthenticationService userAuthenticationService,
    EmailService emailService)
    : IRequestHandler<SendConfirmationEmailCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        var errorOrUser = await unitOfWork.User.GetByEmailAsync(request.Email);

        if (errorOrUser.IsError)
            return Error.Validation("User with such email doesn't exist");

        var user = errorOrUser.Value;

        var token = await userAuthenticationService.GenerateEmailConfirmationTokenAsync(user);

        string? userName;

        if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
        {
            if (string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.FirstName))
            {
                userName = user.Email;
            }
            else if (string.IsNullOrEmpty(user.LastName))
            {
                userName = user.FirstName;
            }
            else
            {
                userName = user.LastName;
            }
        }
        else
        {
            userName = user.FirstName + " " + user.LastName;
        }

        var sendEmailResult = await emailService.SendEmailConfirmationEmailAsync(
            user.Id, user.Email!, token, request.BaseUrl, userName!);

        return sendEmailResult;
    }
}
