using FluentValidation;

namespace Facebook.Application.UserProfile.Command.DeleteAvatar;

public class DeleteAvatarCommandValidator : AbstractValidator<DeleteAvatarCommand>
{
    public DeleteAvatarCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId is required.");
    }
}