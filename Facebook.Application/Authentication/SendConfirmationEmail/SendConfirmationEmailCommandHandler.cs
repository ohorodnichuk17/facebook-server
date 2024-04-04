using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Application.Services;
using MediatR;

namespace Facebook.Application.Authentication.SendConfirmationEmail;

public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand, ErrorOr<Success>>
{
	private readonly IUserRepository _userRepository;
	private readonly IUserAuthenticationService _userAuthenticationService;
	private readonly EmailService _emailService;

	public SendConfirmationEmailCommandHandler(IUserRepository userRepository, 
		IUserAuthenticationService userAuthenticationService, 
		EmailService emailService) 
	{
		_userRepository = userRepository;
		_userAuthenticationService = userAuthenticationService;
		_emailService = emailService;
	}

	public async Task<ErrorOr<Success>> Handle(SendConfirmationEmailCommand command, 
		CancellationToken cancellationToken)
	{
		var errorOrUser = await _userRepository.GetByEmailAsync(command.Email);

		if (errorOrUser.IsError)
			return Error.Validation("User with this email doesn't exists");

		var user = errorOrUser.Value;

		var token = await _userAuthenticationService.GenerateEmailConfirmationTokenAsync(user);

		var sendEmailResult = await _emailService.SendEmailConfirmationEmailAsync(
			user.Id, user.Email!, token, command.BaseUrl);

		return sendEmailResult;
	}
}
