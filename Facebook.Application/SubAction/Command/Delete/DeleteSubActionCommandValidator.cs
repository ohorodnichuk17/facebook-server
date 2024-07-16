using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.SubAction.Command.Delete;

public class DeleteSubActionCommandValidator : AbstractValidator<DeleteSubActionCommand>
{
    public DeleteSubActionCommandValidator()
    {
        RuleFor(r => r.Id)
           .NotEmpty().WithMessage("Id must not be empty");
    }
}
