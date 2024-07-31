using FluentValidation;

namespace Facebook.Application.User.Friends.Command.AcceptFriendRequest;

public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
{
    public AcceptFriendRequestCommandValidator()
    {
        RuleFor(r => r.FriendId)
            .NotEmpty().WithMessage("FriendId must not be empty").When(r => r.FriendId != Guid.Empty);
    }
}