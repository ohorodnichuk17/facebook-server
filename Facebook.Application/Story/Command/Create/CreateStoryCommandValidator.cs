using FluentValidation;

namespace Facebook.Application.Story.Command.Create;

public class CreateStoryCommandValidator : AbstractValidator<CreateStoryCommand>
{
    public CreateStoryCommandValidator()
    {
        RuleFor(r => r.Content)
            .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters.");

        RuleFor(r => r.Image)
            .Custom((image, context) =>
            {
                if (image != null && image.Length > (2 * 1024 * 1024))
                {
                    context.AddFailure("Image", "File size must not exceed 2MB");
                }
            });
    }
}