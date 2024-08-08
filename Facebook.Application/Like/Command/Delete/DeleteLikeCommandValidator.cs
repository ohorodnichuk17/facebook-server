using FluentValidation;

namespace Facebook.Application.Like.Command.Delete;

public class DeleteLikeCommandValidator : AbstractValidator<DeleteLikeCommand>
{
    public DeleteLikeCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("Id is required.");
    }
}
