using FluentValidation;

namespace Facebook.Application.Admin.Query.GetUserById;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty.");
    }
}