using FluentValidation;

namespace Facebook.Application.Story.Command.Delete;

public class DeleteStoryCommandValidator : AbstractValidator<DeleteStoryCommand>
{
    public DeleteStoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}