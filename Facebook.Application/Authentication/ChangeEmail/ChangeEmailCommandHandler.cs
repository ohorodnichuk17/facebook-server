using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using MediatR;
using ErrorOr;
using Facebook.Application.Services;
using Facebook.Domain.User;

namespace Facebook.Application.Authentication.ChangeEmail;

public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, ErrorOr<UserEntity>>
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IUserRepository _userRepository;
    private readonly EmailService _emailService;

    public ChangeEmailCommandHandler(IUserAuthenticationService userAuthenticationService, IUserRepository userRepository, EmailService emailService)
    {
        _userAuthenticationService = userAuthenticationService;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<ErrorOr<UserEntity>> Handle(ChangeEmailCommand request,
        CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetUserByIdAsync(request.UserId);

        if (userResult.IsError)
        {
            return userResult;
        }

        var user = userResult.Value;
        user.Email = request.Email; 

        var changeEmailResult = await _emailService
            .SendChangeEmailEmailAsync(request.Email, request.Token, request.BaseUrl, user.UserName, request.UserId);

        if (changeEmailResult.IsError)
        {
            return changeEmailResult.Errors;
        }

        var resultOfUserToUpdate = await _userRepository.SaveUserAsync(user);

        return resultOfUserToUpdate;
    }

}