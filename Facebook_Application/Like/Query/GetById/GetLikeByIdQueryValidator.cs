using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Like.Query.GetById;

public class GetLikeByIdQueryValidator : AbstractValidator<GetLikeByIdQuery>
{
    public GetLikeByIdQueryValidator()
    {
        RuleFor(r => r.Id)
           .NotEmpty().WithMessage("Id must not be empty");
    }
}
