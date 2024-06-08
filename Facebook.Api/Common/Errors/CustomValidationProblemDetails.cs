using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Facebook.Server.Common.Errors;

public class CustomValidationProblemDetails : ValidationProblemDetails
{
    [JsonPropertyName("errors")]
    public new IEnumerable<ValidationError> Errors { get; }


    public CustomValidationProblemDetails(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }

    public CustomValidationProblemDetails(ModelStateDictionary modelState)
    {
        Errors = ConvertModelStateErrorsToValidationErrors(modelState);
    }

    private List<ValidationError> ConvertModelStateErrorsToValidationErrors(
        ModelStateDictionary modelStateDictionary)
    {
        List<ValidationError> validationErrors = new();

        foreach (var keyModelStatePair in modelStateDictionary)
        {
            var errors = keyModelStatePair.Value.Errors;

            switch (errors.Count)
            {
                case 0:
                    continue;

                case 1:
                    validationErrors.Add(new ValidationError { Code = 100, Message = errors[0].ErrorMessage });
                    break;

                default:
                    var errorMessage = string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage));
                    validationErrors.Add(new ValidationError { Message = errorMessage });
                    break;
            }
        }

        return validationErrors;
    }
}


public class ValidationError
{
    public int Code { get; set; }

    public required string Message { get; set; }
}
