using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Services;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Authentication.ChangeEmail;

public class ChangeEmailCommandHandler(
    IUserAuthenticationService userAuthenticationService,
    IUnitOfWork unitOfWork,
    EmailService emailService)
    : IRequestHandler<ChangeEmailCommand, ErrorOr<UserEntity>>
{
    private readonly IUserAuthenticationService _userAuthenticationService = userAuthenticationService;

    public async Task<ErrorOr<UserEntity>> Handle(ChangeEmailCommand request,
        CancellationToken cancellationToken)
    {
        var userResult = await unitOfWork.User.GetUserByIdAsync(request.UserId);

        if (userResult.IsError)
        {
            return userResult;
        }

        var user = userResult.Value;
        user.Email = request.Email;

        var changeEmailResult = await emailService
            .SendChangeEmailEmailAsync(request.Email, request.Token, request.BaseUrl, user.UserName, request.UserId);

        if (changeEmailResult.IsError)
        {
            return changeEmailResult.Errors;
        }

        var resultOfUserToUpdate = await unitOfWork.User.SaveUserAsync(user);

        return resultOfUserToUpdate;
    }

}