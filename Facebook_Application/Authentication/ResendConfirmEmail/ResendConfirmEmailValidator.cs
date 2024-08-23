using FluentValidation;

namespace Facebook.Application.Authentication.ResendConfirmEmail;

public class ResendConfirmEmailValidator : AbstractValidator<ResendConfirmEmailCommand>
{
    public ResendConfirmEmailValidator()
    {
        RuleFor(r => r.Email).NotEmpty().WithMessage("Field must not be empty");
    }
}