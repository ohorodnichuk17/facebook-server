using FluentValidation;

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
