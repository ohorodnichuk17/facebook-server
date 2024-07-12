using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetReactionByPostId;

public class GetReactionsByPostIdQueryValidator : AbstractValidator<GetReactionsByPostIdQuery>
{
    public GetReactionsByPostIdQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty()
            .WithMessage("Post id is required.");
    }
}
