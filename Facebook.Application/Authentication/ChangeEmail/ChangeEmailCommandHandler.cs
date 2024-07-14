using Facebook.Application.Common.Interfaces.Authentication;
using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Application.Services;
using Facebook.Domain.User;

namespace Facebook.Application.Authentication.ChangeEmail;

public class ChangeEmailCommandHandler(
    IUserAuthenticationService userAuthenticationService,
    IUserRepository userRepository,
    EmailService emailService)
    : IRequestHandler<ChangeEmailCommand, ErrorOr<UserEntity>>
{
    private readonly IUserAuthenticationService _userAuthenticationService = userAuthenticationService;

    public async Task<ErrorOr<UserEntity>> Handle(ChangeEmailCommand request,
        CancellationToken cancellationToken)
    {
        var userResult = await userRepository.GetUserByIdAsync(request.UserId);

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

        var resultOfUserToUpdate = await userRepository.SaveUserAsync(user);

        return resultOfUserToUpdate;
    }

}