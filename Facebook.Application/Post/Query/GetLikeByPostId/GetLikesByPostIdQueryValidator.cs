using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetLikeByPostId;

public class GetLikesByPostIdQueryValidator : AbstractValidator<GetLikesByPostIdQuery>
{
    public GetLikesByPostIdQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty()
            .WithMessage("Post id is required.");
    }
}
