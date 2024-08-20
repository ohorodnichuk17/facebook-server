using FluentValidation;

namespace Facebook.Application.User.Friends.Command.RemoveFriend;

public class RemoveFriendCommandValidator : AbstractValidator<RemoveFriendCommand>
{
    public RemoveFriendCommandValidator()
    {
        RuleFor(x => x.FriendId)
            .NotEmpty()
            .WithMessage("FriendId is required.");
    }
}