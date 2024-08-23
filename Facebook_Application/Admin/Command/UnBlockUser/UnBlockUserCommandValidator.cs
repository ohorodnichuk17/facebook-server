using Facebook.Application.Admin.Command.BlockUser;
using FluentValidation;

namespace Facebook.Application.Admin.Command.UnBlockUser;

public class UnBlockUserCommandValidator : AbstractValidator<BlockUserCommand>
{
    public UnBlockUserCommandValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty");
    }
}