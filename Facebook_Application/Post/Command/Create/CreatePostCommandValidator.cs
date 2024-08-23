using Facebook.Domain.Constants.ContentVisibility;
using FluentValidation;

namespace Facebook.Application.Post.Command.Create;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(r => r.Title)
            .MaximumLength(10).WithMessage("Title must not exceed 10 characters.");

        RuleFor(r => r.Content)
            .MaximumLength(3000).WithMessage("Content must not exceed 3000 characters.");

        RuleFor(r => r.Tags)
            .Must(tags => tags == null || tags.All(tag => !string.IsNullOrWhiteSpace(tag)))
            .WithMessage("Tags must not contain empty or whitespace strings");

        RuleFor(r => r.Location)
            .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

        RuleFor(r => r.Images)
            .Must(images => images == null || images.All(image => image.Image != null))
            .WithMessage("Images collection must not contain null items.");

        RuleFor(r => r.Visibility)
            .NotEmpty().WithMessage("Limited post display is required")
            .Must(v => v == ContentVisibility.Public
            || v == ContentVisibility.FriendsOnly
            || v == ContentVisibility.Private
            || v == ContentVisibility.FriendsExcept);
    }
}