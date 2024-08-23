using FluentValidation;

namespace Facebook.Application.Post.Query.SearchPostsByTags;

public class SearchPostsByTagsQueryValidator : AbstractValidator<SearchPostsByTagsQuery>
{
    public SearchPostsByTagsQueryValidator()
    {
        RuleFor(x => x.Tag)
        .NotEmpty()
        .WithMessage("Tag is required.");
    }
}