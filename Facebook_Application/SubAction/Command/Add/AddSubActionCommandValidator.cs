using FluentValidation;

namespace Facebook.Application.SubAction.Command.Add;

public class AddSubActionCommandValidator : AbstractValidator<AddSubActionCommand>
{
    public AddSubActionCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name must not be empty");

        RuleFor(r => r.ActionId)
           .NotEmpty().WithMessage("Action id must not be empty");
    }
}
