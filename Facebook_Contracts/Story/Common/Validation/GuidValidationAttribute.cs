using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Story.Common.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class GuidValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (Guid.TryParse(value.ToString(), out _))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("{PropertyName} is not a valid GUID.");
    }
}