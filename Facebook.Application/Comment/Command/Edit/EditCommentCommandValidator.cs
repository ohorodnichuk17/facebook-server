using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.Edit;

public class EditCommentCommandValidator : AbstractValidator<EditCommentCommand>
{
    public EditCommentCommandValidator()
    {
        RuleFor(r => r.Message)
          .MaximumLength(100).WithMessage("Message must not exceed 100 characters.");

        RuleFor(r => r.Id)
          .NotEmpty().WithMessage("Comment id is required.");
    }
}
