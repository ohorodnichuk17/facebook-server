using FluentValidation;

namespace Facebook.Application.User.Friends.Command.RejectFriendRequest;

public class RejectFriendRequestCommandValidator : AbstractValidator<RejectFriendRequestCommand>
{
    public RejectFriendRequestCommandValidator()
    {
        RuleFor(r => r.FriendId)
            .NotEmpty().WithMessage("FriendRequestId must not be empty").When(r => r.FriendId != Guid.Empty);
    }
}
