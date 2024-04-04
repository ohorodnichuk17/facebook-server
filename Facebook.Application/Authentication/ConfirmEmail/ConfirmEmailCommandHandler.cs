using Facebook.Application.Common.Interfaces.Authentication;
using ErrorOr;
using MediatR;

namespace Facebook.Application.Authentication.ConfirmEmail;

public class ConfirmEmailCommandHandler :
	IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
{
	private readonly IUserAuthenticationService _userAuthenticationService;

	public ConfirmEmailCommandHandler(IUserAuthenticationService userAuthenticationService)
	{
		_userAuthenticationService = userAuthenticationService;
	}

	public async Task<ErrorOr<Success>> Handle(
		ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		var errorOrSuccess = await _userAuthenticationService.ConfirmEmailAsync(request.UserId,
			request.Token);

		return errorOrSuccess;
	}
}
