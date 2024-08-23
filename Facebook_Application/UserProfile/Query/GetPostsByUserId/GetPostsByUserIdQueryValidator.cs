using FluentValidation;

namespace Facebook.Application.UserProfile.Query.GetPostsByUserId;

public class GetPostsByUserIdQueryValidator : AbstractValidator<GetPostsByUserIdQuery>
{
    public GetPostsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.");
    }
}
