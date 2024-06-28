using FluentValidation;

namespace Facebook.Application.Authentication.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
	public ConfirmEmailCommandValidator()
	{
		RuleFor(r => r.UserId).NotEmpty().WithMessage("Field must not be empty");

		RuleFor(r => r.ValidEmailToken).NotEmpty().WithMessage("Field must not be empty")
			.MaximumLength(4096).MinimumLength(256);
	}
}
