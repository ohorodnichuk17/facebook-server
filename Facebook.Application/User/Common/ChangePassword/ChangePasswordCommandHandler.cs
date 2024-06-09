using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Common.ChangePassword;

public class ChangePasswordCommandHandler
	: IRequestHandler<ChangePasswordCommand, ErrorOr<UserEntity>>
{
	private readonly IUserRepository _userRepository;
	private readonly IUserAuthenticationService _userAuthenticationService;

	public ChangePasswordCommandHandler(IUserRepository userRepository, IUserAuthenticationService userAuthenticationService)
	{
		_userRepository = userRepository;
		_userAuthenticationService = userAuthenticationService;
	}

	public async Task<ErrorOr<UserEntity>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
	{
		var userOrError = await _userRepository.GetUserByIdAsync(request.UserId);
		if (userOrError.IsError)
			return userOrError;
		
		var user = userOrError.Value;
		var changeUserResult = await _userAuthenticationService.ChangePasswordAsync(
			user, request.CurrentPassword, request.NewPassword);

		return changeUserResult;
	}
}
