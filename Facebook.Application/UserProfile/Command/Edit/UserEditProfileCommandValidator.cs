using FluentValidation;

namespace Facebook.Application.UserProfile.Command.Edit;

public class UserEditProfileCommandValidator : AbstractValidator<UserEditProfileCommand>
{
   public UserEditProfileCommandValidator()
   {
      RuleFor(r => r.Biography)
         .MaximumLength(100).WithMessage("Biography must not exceed 100 characters.")
         .MinimumLength(5).WithMessage("Biography must be at least 5 characters.")
         .When(r => !string.IsNullOrEmpty(r.Biography));

      RuleFor(r => r.FirstName)
          .MaximumLength(100).WithMessage("First name must not exceed 100 characters.")
          .MinimumLength(5).WithMessage("First name must be at least 5 characters.")
          .When(r => !string.IsNullOrEmpty(r.FirstName));

      RuleFor(r => r.LastName)
          .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.")
          .MinimumLength(5).WithMessage("Last name must be at least 5 characters.")
          .When(r => !string.IsNullOrEmpty(r.LastName));

      RuleFor(r => r.UserId)
          .NotEmpty().WithMessage("User ID is required.");

      RuleFor(r => r.Country)
          .MaximumLength(100).WithMessage("Country must not exceed 100 characters.")
          .MinimumLength(5).WithMessage("Country must be at least 5 characters.")
          .When(r => !string.IsNullOrEmpty(r.Country));

      RuleFor(r => r.Pronouns)
          .Must(pronouns => pronouns == "she/her" || pronouns == "he/him" || pronouns == "they/them" || pronouns == "do not specify")
          .WithMessage("Invalid pronouns value.")
          .When(r => !string.IsNullOrEmpty(r.Pronouns));


      RuleFor(r => r.Region)
          .MaximumLength(100).WithMessage("Region must not exceed 100 characters.")
          .MinimumLength(5).WithMessage("Region must be at least 5 characters.")
          .When(r => !string.IsNullOrEmpty(r.Region));


      RuleFor(r => r.CoverPhoto).Custom((image, context) =>
      {
         if (image != null && image.Length > 2 * 1024 * 1024)
         {
            context.AddFailure("Image", "File size must not exceed 2MB");
         }
      });

      RuleFor(r => r.Avatar).Custom((image, context) =>
      {
         if (image != null && image.Length > 2 * 1024 * 1024)
         {
            context.AddFailure("Image", "File size must not exceed 2MB");
         }
      });
   }
}
