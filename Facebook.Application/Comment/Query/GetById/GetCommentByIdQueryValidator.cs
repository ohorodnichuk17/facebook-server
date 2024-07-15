using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Query.GetById;

public class GetCommentByIdQueryValidator : AbstractValidator<GetCommentByIdQuery>
{
    public GetCommentByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}
