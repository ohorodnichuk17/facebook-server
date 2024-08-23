using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Query.GetById;

public class GetReactionByIdQueryValidator : AbstractValidator<GetReactionByIdQuery>
{
    public GetReactionByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}
