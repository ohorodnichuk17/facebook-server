using Facebook.Application.Feeling.Command.Delete;
using FluentValidation;

namespace Facebook.Application.Feeling.Query.GetById;

public class GetFeelingByIdQueryValidator : AbstractValidator<GetFeelingByIdQuery>
{
    public GetFeelingByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}