using FluentValidation;

namespace Facebook.Application.UserProfile.Command.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId must not be empty").When(r => r.UserId != null);
    }
}

