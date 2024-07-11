using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Command.Delete;

public class DeleteReactionCommandValidator : AbstractValidator<DeleteReactionCommand>
{
    public DeleteReactionCommandValidator()
    {
        RuleFor(r => r.Id)
           .NotEmpty().WithMessage("Id must not be empty").When(r => r.Id != Guid.Empty);

    }
}
