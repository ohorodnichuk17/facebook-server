using FluentValidation;

namespace Facebook.Application.Comment.Command.AddReplyComment;

public class AddReplyCommentCommandValidator : AbstractValidator<AddReplyCommentCommand>
{
    public AddReplyCommentCommandValidator()
    {
        RuleFor(r => r.Message)
            .NotEmpty().MaximumLength(250).WithMessage("Message must not exceed 250 characters.");

        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("Parent id is required.");
    }
}
