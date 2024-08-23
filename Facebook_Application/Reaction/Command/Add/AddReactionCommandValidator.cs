using FluentValidation;

namespace Facebook.Application.Reaction.Command.Add;

public class AddReactionCommandValidator : AbstractValidator<AddReactionCommand>
{
    public AddReactionCommandValidator()
    {
        RuleFor(r => r.PostId)
           .NotEmpty().WithMessage("PostId must not be empty").When(r => r.PostId != Guid.Empty);

        RuleFor(r => r.Emoji)
           .NotEmpty().MaximumLength(1000).WithMessage("TypeCode must not exceed 1000 characters.");
    }
}
