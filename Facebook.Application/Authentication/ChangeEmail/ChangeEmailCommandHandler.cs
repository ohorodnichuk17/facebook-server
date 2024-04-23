using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.UserEntity;
using MediatR;
using ErrorOr;
using Facebook.Application.Services;

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

    // public ChangeEmailCommandHandler(IUserAuthenticationService userAuthenticationService, IUserRepository userRepository)
    // {
    //     _userAuthenticationService = userAuthenticationService;
    //     _userRepository = userRepository;
    // }

    public async Task<ErrorOr<UserEntity>> Handle(ChangeEmailCommand request,
        CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetUserByIdAsync(request.UserId);

        if (userResult.IsError)
        {
            return userResult;
        }

        var user = userResult.Value;
        // var changeEmailResult = await _userAuthenticationService.ChangeEmailAsync(user,
        //     request.Email, request.Token);
        
        user.UserName = request.Email;
        var userName = user.NormalizedUserName = request.Email.ToLower();

        var changeEmailResult = await _emailService
            .SendChangeEmailEmailAsync(request.Email, request.Token, request.BaseUrl, userName, request.UserId);

        if (changeEmailResult.IsError)
        {
            return changeEmailResult.Errors;
        }

        // user.UserName = request.Email;
        // user.NormalizedUserName = request.Email.ToLower();

        var resultOfUserToUpdate = await _userRepository.SaveUserAsync(user);

        return resultOfUserToUpdate;
    }
}