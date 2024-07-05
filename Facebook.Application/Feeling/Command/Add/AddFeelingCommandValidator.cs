using FluentValidation;

namespace Facebook.Application.Feeling.Command.Add;

public class AddFeelingCommandValidator : AbstractValidator<AddFeelingCommand>
{
    public AddFeelingCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name must not be empty");
        
        RuleFor(r => r.Emoji)
            .NotEmpty().WithMessage("Emoji must not be empty");

    }
}