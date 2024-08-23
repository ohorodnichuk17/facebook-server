using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Command.Edit;

public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
{
    public EditMessageCommandValidator()
    {
        RuleFor(r => r.Content)
          .MaximumLength(100).WithMessage("Content must not exceed 100 characters.");

        RuleFor(r => r.Id)
          .NotEmpty().WithMessage("Message id is required.");
    }
}
