using FluentValidation;

namespace Facebook.Application.UserProfile.Query.GetStoriesByUserId;

public class GetStoriesByUserIdQueryValidator : AbstractValidator<GetStoriesByUserIdQuery>
{
    public GetStoriesByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty()
           .WithMessage("User Id is required.");
    }
}
