using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.UserEntity;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Authentication.ChangeEmail;

public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, ErrorOr<UserEntity>>
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IUserRepository _userRepository;
    public async Task<ErrorOr<UserEntity>> Handle(ChangeEmailCommand request,
        CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetUserByIdAsync(request.UserId);

        if (userResult.IsError)
        {
            return userResult;
        }

        var user = userResult.Value;
        var changeEmailResult = await _userAuthenticationService.ChangeEmailAsync(user,
            request.Email, request.Token);

        if (changeEmailResult.IsError)
        {
            return changeEmailResult;
        }

        user.UserName = request.Email;
        user.NormalizedUserName = request.Email.ToLower();

        var resultOfUserToUpdate = await _userRepository.SaveUserAsync(user);

        return resultOfUserToUpdate;
    }
}