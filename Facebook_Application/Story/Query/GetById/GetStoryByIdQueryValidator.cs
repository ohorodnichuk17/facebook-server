using FluentValidation;

namespace Facebook.Application.Story.Query.GetById;

public class GetStoryByIdQueryValidator : AbstractValidator<GetStoryByIdQuery>
{
    public GetStoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}