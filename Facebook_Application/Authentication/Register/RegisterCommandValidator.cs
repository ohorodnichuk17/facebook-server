using FluentValidation;

namespace Facebook.Application.Authentication.Register;

public class RegisterCommandValidator :  AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(r => r.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");
        
        RuleFor(r => r.Email).NotEmpty().WithMessage("Field must not be empty")
            .EmailAddress().WithMessage("Invalid email format")
            .MinimumLength(5)
            .MaximumLength(254);

        RuleFor(r => r.Password).NotEmpty().WithMessage("Field must not be empty")
            .MaximumLength(24).MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain one or more capital letters.")
            .Matches("[a-z]").WithMessage("Password must contain one or more lowercase letters.")
            .Matches(@"\d").WithMessage("Password must contain one or more digits.")
            .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("Password must contain one or more special characters.")
            .Matches("^[^£# “”]*$").WithMessage("Password must not contain the following characters £ # “” or spaces."); ;

        RuleFor(r => r.ConfirmPassword).NotEmpty().WithMessage("Required field must not be empty.")
            .Equal(r => r.Password).WithMessage("Passwords are not matched");

        RuleFor(r => r.Birthday)
            .Must(BeAValidDate).WithMessage("Invalid date format.")
            .LessThan(DateTime.Now).WithMessage("Birthday must be in the past.");

        RuleFor(r => r.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(gender => gender == "Male" || gender == "Female" || gender == "Other").WithMessage("Invalid gender value.");
        
        RuleFor(command => command.Avatar)
            .Custom((avatar, context) =>
            {
                if (avatar != null) 
                {
                    if (avatar.Length > (2 * 1024 * 1024))
                    {
                        context.AddFailure("Avatar", "File size must not exceed 2MB");
                    }
                }
            });

    }
    
    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }

    private static bool IsFileExtensionValid(string filename)
    {
        var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
        var fileExtension = Path.GetExtension(filename).ToLower();
        return allowedExtensions.Contains(fileExtension);
    }
}