using FluentValidation;

namespace Facebook.Application.User.Friends.Command.RejectFriendRequest;

public class RejectFriendRequestCommandValidator : AbstractValidator<RejectFriendRequestCommand>
{
    public RejectFriendRequestCommandValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("UserId must not be empty").When(r => r.UserId != Guid.Empty);

        RuleFor(r => r.FriendRequestId)
            .NotEmpty().WithMessage("FriendRequestId must not be empty").When(r => r.UserId != Guid.Empty);

    }
}
