using FluentValidation;

namespace Facebook.Application.Action.Command.Add;

public class AddActionCommandValidator : AbstractValidator<AddActionCommand>
{
    public AddActionCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name must not be empty");

        RuleFor(r => r.Emoji)
            .NotEmpty().WithMessage("Emoji must not be empty");
    }
}
