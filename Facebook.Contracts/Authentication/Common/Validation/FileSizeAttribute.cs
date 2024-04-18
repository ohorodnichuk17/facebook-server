using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Facebook.Contracts.Authentication.Common.Validation;

public class FileSizeAttribute : ValidationAttribute
{
    private readonly long _maxFileSize;

    public FileSizeAttribute(long maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return new ValidationResult("Invalid file");
        }

        if (file.Length > _maxFileSize)
        {
            return new ValidationResult($"The file size can not exceed {_maxFileSize / 1024 / 1024} MB");
        }

        return ValidationResult.Success!;
    }
}