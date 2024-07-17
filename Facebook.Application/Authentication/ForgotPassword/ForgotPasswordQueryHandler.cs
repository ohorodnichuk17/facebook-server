using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Services;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Authentication.ForgotPassword;

public class ForgotPasswordQueryHandler(
    IUnitOfWork unitOfWork,
    IUserAuthenticationService userAuthenticationService,
    EmailService emailService)
        : IRequestHandler<ForgotPasswordQuery, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        var errorOrUser = await unitOfWork.User.GetByEmailAsync(request.Email);
        if (errorOrUser.IsError)
        {
            return Error.Validation("User with this email doesn't exist");
        }

        var user = errorOrUser.Value;
        string token = await userAuthenticationService.GeneratePasswordResetTokenAsync(user);

        string userName = GetUserNameForEmail(user);
        var sendEmailResult = await emailService
            .SendForgotPasswordEmailAsync(user.Email!, token, request.BaseUrl, userName);

        return sendEmailResult;
    }

    private string GetUserNameForEmail(UserEntity user)
    {
        if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
        {
            return user.FirstName + " " + user.LastName;
        }
        if (string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.FirstName))
        {
            return user.Email;
        }
        else if (string.IsNullOrEmpty(user.LastName))
        {
            return user.FirstName;
        }
        else
        {
            return user.LastName;
        }
    }
}