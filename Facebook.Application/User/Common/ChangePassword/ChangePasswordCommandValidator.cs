using Facebook.Application.Users.Common.ChangePassword;
using FluentValidation;

namespace Facebook.Application.User.Common.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
		RuleFor(r => r.CurrentPassword).NotEmpty().WithMessage("{PropertyName} must not be empty")
			 .MaximumLength(24).MinimumLength(8)
			 .Matches("[A-Z]").WithMessage("{PropertyName} must contain one or more capital letters.")
			 .Matches("[a-z]").WithMessage("{PropertyName} must contain one or more lowercase letters.")
			 .Matches(@"\d").WithMessage("{PropertyName} must contain one or more digits.")
			 .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("{PropertyName} must contain one or more special characters.")
			 .Matches("^[^£# “”]*$").WithMessage("{PropertyName} must not contain the following characters £ # “” or spaces.");

		RuleFor(r => r.NewPassword).NotEmpty().WithMessage("{PropertyName} must not be empty")
			 .MaximumLength(24).MinimumLength(8)
			 .Matches("[A-Z]").WithMessage("{PropertyName} must contain one or more capital letters.")
			 .Matches("[a-z]").WithMessage("{PropertyName} must contain one or more lowercase letters.")
			 .Matches(@"\d").WithMessage("{PropertyName} must contain one or more digits.")
			 .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("{PropertyName} must contain one or more special characters.")
			 .Matches("^[^£# “”]*$").WithMessage("{PropertyName} must not contain the following characters £ # “” or spaces.");

		RuleFor(r => r.ConfirmNewPassword).NotEmpty().WithMessage("Required field must not be empty.")
			.Equal(r => r.NewPassword).WithMessage("Passwords are not matched");

		RuleFor(r => r.UserId).NotEmpty().WithMessage("Field must not be empty")
		.MaximumLength(36).MinimumLength(36);
	}
}
