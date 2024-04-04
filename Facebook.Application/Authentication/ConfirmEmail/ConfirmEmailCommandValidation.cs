using FluentValidation;

namespace Facebook.Application.Authentication.ConfirmEmail;

public class ConfirmEmailCommandValidation : AbstractValidator<ConfirmEmailCommand>
{
	public ConfirmEmailCommandValidation()
	{
		RuleFor(r => r.UserId).NotEmpty().WithMessage("Field must not be empty");

		RuleFor(r => r.Token).NotEmpty().WithMessage("Field must not be empty")
			.MaximumLength(4096).MinimumLength(256);
	}
}
