using System.Diagnostics;
using ErrorOr;
using Facebook.Server.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Facebook.Server.Common.Errors;

public class FacebookProblemDetailsFactory(IOptions<ApiBehaviorOptions> options) : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

    public override ProblemDetails CreateProblemDetails(
		HttpContext httpContext,
		int? statusCode = null,
		string? title = null,
		string? type = null,
		string? detail = null,
		string? instance = null)
	{
		statusCode ??= 500;

		var problemDetails = new ProblemDetails
		{
			Status = statusCode,
			Title = title,
			Type = type,
			Detail = detail,
			Instance = instance
		};

		ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

		return problemDetails;
	}

	public override ValidationProblemDetails CreateValidationProblemDetails(
		HttpContext httpContext,
	   ModelStateDictionary modelStateDictionary,
	   int? statusCode = null,
	   string? title = null,
	   string? type = null,
	   string? detail = null,
	   string? instance = null)
	{
		statusCode ??= 400;
		type ??= "https://tools.ietf.org/html/rfc7231#section-6.5.1";
		instance ??= httpContext.Request.Path;

		var problemDetails = new CustomValidationProblemDetails(modelStateDictionary)
		{
			Status = statusCode,
			Type = type,
			Instance = instance
		};

		if (title != null)
		{
			problemDetails.Title = title;
		}

		var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

		if (traceId != null)
		{
			problemDetails.Extensions["traceId"] = traceId;
		}

		return problemDetails;
	}

	private void ApplyProblemDetailsDefaults(
		HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
	{
		problemDetails.Status ??= statusCode;

		if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
		{
			problemDetails.Title ??= clientErrorData.Title;
			problemDetails.Type ??= clientErrorData.Link;
		}

		var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

		if (traceId != null)
		{
			problemDetails.Extensions["traceId"] = traceId;
		}

		var errors = httpContext?.Items["errors"] as List<Error>;

		if (errors is not null)
		{
			problemDetails.Extensions.Add("errorCodes", "customValue");
		}

	}
}