using FluentValidation;

namespace Facebook.Application.Reaction.Command.Delete;

public class DeleteReactionCommandValidator : AbstractValidator<DeleteReactionCommand>
{
    public DeleteReactionCommandValidator()
    {
        RuleFor(r => r.Id)
           .NotEmpty().WithMessage("Id must not be empty").When(r => r.Id != Guid.Empty);
    }
}
