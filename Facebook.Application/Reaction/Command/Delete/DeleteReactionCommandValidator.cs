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
        RuleFor(r => r.UserId)
           .NotEmpty().WithMessage("UserId must not be empty").When(r => r.UserId != Guid.Empty);

        RuleFor(r => r.PostId)
           .NotEmpty().WithMessage("UserId must not be empty").When(r => r.PostId != Guid.Empty);
    }
}
