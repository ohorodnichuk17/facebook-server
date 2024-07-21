using FluentValidation;

namespace Facebook.Application.Post.Query.GetCommentByPostId;

public class GetCommentsByPostIdQueryValidator : AbstractValidator<GetCommentsByPostIdQuery>
{
    public GetCommentsByPostIdQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty()
            .WithMessage("Post id is required.");
    }
}
