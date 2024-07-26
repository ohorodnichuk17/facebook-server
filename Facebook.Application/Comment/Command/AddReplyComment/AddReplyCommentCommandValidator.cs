using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.AddReplyComment;

public class AddReplyCommentCommandValidator : AbstractValidator<AddReplyCommentCommand>
{
    public AddReplyCommentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");

        RuleFor(x => x.PostId)
            .NotEmpty()
            .WithMessage("Post id is required.");

        RuleFor(r => r.Message)
            .NotEmpty().MaximumLength(250).WithMessage("Message must not exceed 250 characters.");

        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("Parent id is required.");
    }
}
