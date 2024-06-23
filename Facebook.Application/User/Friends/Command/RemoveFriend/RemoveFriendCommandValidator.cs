using FluentValidation;

namespace Facebook.Application.User.Friends.Command.RemoveFriend;

public class RemoveFriendCommandValidator : AbstractValidator<RemoveFriendCommand>
{
    public RemoveFriendCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
        
        RuleFor(x => x.FriendId)
            .NotEmpty()
            .WithMessage("FriendId is required.");
    }
}