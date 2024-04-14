using Facebook.Domain.UserEntity;
using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;

namespace Facebook.Application.Authentication.ResetPassword;

public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, ErrorOr<UserEntity>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public ResetPasswordCommandHandler(IUserRepository userRepository, IUserAuthenticationService userAuthenticationService)
    {
        _userRepository = userRepository;
        _userAuthenticationService = userAuthenticationService;
    }
    
    public async Task<ErrorOr<UserEntity>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var errorOrUser = await _userRepository.GetByEmailAsync(request.Email);
        if (errorOrUser.IsError)
        {
            return Error.Validation("User with this email doesn't exist");
        }

        var user = errorOrUser.Value;
        var resetPasswordResult = await _userAuthenticationService
            .ResetPasswordAsync(user, request.Token, request.Password);

        return resetPasswordResult;
    }
}