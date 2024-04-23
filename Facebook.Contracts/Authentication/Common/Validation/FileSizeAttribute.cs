using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

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
        if (value is not byte[] file)
        {
            return ValidationResult.Success;
        }

        if (file.Length > _maxFileSize)
        {
            return new ValidationResult($"The file size can not exceed {_maxFileSize / 1024 / 1024} MB");
        }

        return ValidationResult.Success!;
    }
}
