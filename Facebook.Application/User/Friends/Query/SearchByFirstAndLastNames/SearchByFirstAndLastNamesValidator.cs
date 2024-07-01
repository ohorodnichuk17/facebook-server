using FluentValidation;

namespace Facebook.Application.User.Friends.Query.SearchByFirstAndLastNames;

public class SearchByFirstAndLastNamesValidator: AbstractValidator<SearchByFirstAndLastNamesQuery>
{
    public SearchByFirstAndLastNamesValidator()
    {
        // RuleFor(x => x.UserId)
        //     .NotEmpty()
        //     .WithMessage("UserId is required.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required.");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required.");
    }
}