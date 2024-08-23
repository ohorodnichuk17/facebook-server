using FluentValidation;

namespace Facebook.Application.Like.Command.DeleteByPostId;

public class DeleteLikeByPostIdCommandValidator : AbstractValidator<DeleteLikeByPostIdCommand>
{
    public DeleteLikeByPostIdCommandValidator()
    {
        RuleFor(x => x.PostId)
        .NotEmpty()
        .WithMessage("Id is required.");
    }
}
