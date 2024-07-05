using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Command.Add;

public class AddReactionCommandValidator : AbstractValidator<AddReactionCommand>
{
    public AddReactionCommandValidator()
    {
        RuleFor(r => r.UserId)
           .NotEmpty().WithMessage("UserId must not be empty").When(r => r.UserId != Guid.Empty);

        RuleFor(r => r.PostId)
           .NotEmpty().WithMessage("PostId must not be empty").When(r => r.PostId != Guid.Empty);

        RuleFor(r => r.TypeCode)
            .MaximumLength(1000).WithMessage("TypeCode must not exceed 1000 characters.");
    }
}
