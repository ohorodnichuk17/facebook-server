using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[ApiController]
[Authorize]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        HttpContext.Items["errors"] = errors;

        var firstError = errors.FirstOrDefault();

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "Multiple errors occurred", 
        };

        problemDetails.Extensions["errors"] = errors;

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
        };
    }
}