using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetPostAccess;

public class GetPostAccessQueryValidator : AbstractValidator<GetPostAccessQuery>
{
    public GetPostAccessQueryValidator()
    {
        RuleFor(x => x.ViewerId)
            .NotEmpty()
            .WithMessage("Viewer id is required.");

        RuleFor(x => x.PostId)
            .NotEmpty()
            .WithMessage("Post id is required.");
    }
}
