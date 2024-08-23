using FluentValidation;

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
