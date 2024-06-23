using FluentValidation;

namespace Facebook.Application.User.Friends.Command.AcceptFriendRequest;

public class AcceptFriendRequestCommandValidator : AbstractValidator<AcceptFriendRequestCommand>
{
    public AcceptFriendRequestCommandValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("UserId must not be empty").When(r => r.UserId != Guid.Empty);

        RuleFor(r => r.FriendRequestId)
            .NotEmpty().WithMessage("FriendRequestId must not be empty").When(r => r.UserId != Guid.Empty);

    }
}