using FluentValidation;

namespace Facebook.Application.User.Friends.Query.GetById;

public class GetFriendByIdQueryValidator : AbstractValidator<GetFriendByIdQuery>
{
    public GetFriendByIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
        
        RuleFor(x => x.FriendId)
            .NotEmpty()
            .WithMessage("FriendId is required.");
    }
}