using FluentValidation;

namespace Facebook.Application.Admin.Command.UnBanUser;

public class UnBanUserCommandValidator : AbstractValidator<UnBanUserCommand>
{
    public UnBanUserCommandValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty");
    }
}