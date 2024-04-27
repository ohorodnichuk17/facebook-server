using FluentValidation;

namespace Facebook.Application.Authentication.ForgotPassword;

public class ForgotPasswordQueryValidator 
    : AbstractValidator<ForgotPasswordQuery>
{
    public ForgotPasswordQueryValidator()
    {
        RuleFor(r => r.Email).NotEmpty()
            .WithMessage("Field must not be empty").EmailAddress()
            .WithMessage("Wrong email format").MaximumLength(254)
            .MinimumLength(5);
   

        // RuleFor(r => r.BaseUrl).NotEmpty()
        //     .WithMessage("Required field must not be empty");
    }
}