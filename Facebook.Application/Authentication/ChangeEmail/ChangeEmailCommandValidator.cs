using FluentValidation;

namespace Facebook.Application.Authentication.ChangeEmail;

public class ChangeEmailCommandValidator : AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty()
            .WithMessage("Field must not be empty").MaximumLength(36)
            .MinimumLength(36);

        RuleFor(r => r.Email).NotEmpty()
            .WithMessage("Field must not be empty").EmailAddress()
            .WithMessage("Wrong email format").MaximumLength(254)
            .MinimumLength(5);

        RuleFor(r => r.Token).NotEmpty()
            .WithMessage("Field must not be empty").MaximumLength(4096)
            .MinimumLength(256);
    }
}