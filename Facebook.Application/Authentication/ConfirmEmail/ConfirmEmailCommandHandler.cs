using Facebook.Application.Common.Interfaces.Authentication;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using MediatR;

namespace Facebook.Application.Authentication.ConfirmEmail;

public class ConfirmEmailCommandHandler :
	IRequestHandler<ConfirmEmailCommand, ErrorOr<string>>
{
	private readonly IUserAuthenticationService _userAuthenticationService;
	private IJwtGenerator _jwtGenerator;
	private readonly IUserRepository _userRepository;

	public ConfirmEmailCommandHandler(IUserAuthenticationService userAuthenticationService, IJwtGenerator jwtGenerator, IUserRepository userRepository)
	{
		_userAuthenticationService = userAuthenticationService;
		_jwtGenerator = jwtGenerator;
		_userRepository = userRepository;
	}

	public async Task<ErrorOr<string>> Handle(
		ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		var errorOrSuccess = await _userAuthenticationService
			.ConfirmEmailAsync(request.UserId, request.Token);
		if (errorOrSuccess.IsError)
		{
			return errorOrSuccess.Errors;
		}

		var userOrError = await _userRepository.GetUserByIdAsync(request.UserId.ToString());
		if (userOrError.IsError)
		{
			return userOrError.Errors;
		}

		var user = userOrError.Value;
		var roleOrError = await _userRepository.FindRolesByUserIdAsync(user);
		if (roleOrError.IsError)
		{
			return roleOrError.Errors;
		}

		var role = roleOrError.Value.First();
		var token =  await _jwtGenerator.GenerateJwtTokenAsync(user, role);

		return token;
	}
}
