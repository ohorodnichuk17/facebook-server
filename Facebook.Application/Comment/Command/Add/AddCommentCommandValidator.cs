using Facebook.Application.Story.Command.Delete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.Add;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");
        RuleFor(x => x.PostId)
                .NotEmpty()
                .WithMessage("Post id is required.");
        RuleFor(r => r.Message)
            .MaximumLength(250).WithMessage("Message must not exceed 250 characters.");
    }
}
