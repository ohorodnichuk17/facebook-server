using FluentValidation;

namespace Facebook.Application.Comment.Command.Add;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.PostId)
                .NotEmpty()
                .WithMessage("Post id is required.");

        RuleFor(r => r.Message)
            .NotEmpty().MaximumLength(250).WithMessage("Message must not exceed 250 characters.");
    }
}
