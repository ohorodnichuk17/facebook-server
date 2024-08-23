using Facebook.Application.Admin.Command.BlockUser;
using FluentValidation;

namespace Facebook.Application.Admin.Command.BanUser;

public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
{
    public BanUserCommandValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty");
    }
}