using FluentValidation;

namespace Facebook.Application.UserProfile.Command.DeleteCoverPhoto;

public class DeleteCoverPhotoCommandValidator : AbstractValidator<DeleteCoverPhotoCommand>
{
    public DeleteCoverPhotoCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId is required.");
    }
}