using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Command.Delete;

public class DeleteMessageByIdCommandValidator : AbstractValidator<DeleteMessageByIdCommand>
{
    public DeleteMessageByIdCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}
