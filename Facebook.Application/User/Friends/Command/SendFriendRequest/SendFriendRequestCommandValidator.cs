using FluentValidation;

namespace Facebook.Application.User.Friends.Command.SendFriendRequest;

public class SendFriendRequestCommandValidator : AbstractValidator<SendFriendRequestCommand>
{
    public SendFriendRequestCommandValidator()
    {
        RuleFor(r => r.FriendId)
            .NotEmpty().WithMessage("FriendId must not be empty").When(r => r.FriendId != Guid.Empty);
    }
}