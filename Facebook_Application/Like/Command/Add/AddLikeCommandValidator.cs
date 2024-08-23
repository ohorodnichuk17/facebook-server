using FluentValidation;

namespace Facebook.Application.Like.Command.Add;

public class AddLikeCommandValidator : AbstractValidator<AddLikeCommand>
{
    public AddLikeCommandValidator()
    {
        RuleFor(x => x.PostId)
                .NotEmpty()
                .WithMessage("Post id is required.");
    }
}
