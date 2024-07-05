using FluentValidation;

namespace Facebook.Application.Feeling.Command.Delete;

public class DeleteFeelingCommandValidator : AbstractValidator<DeleteFeelingCommand>
{
    public DeleteFeelingCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}