using System.ComponentModel.DataAnnotations;
using Facebook.Contracts.Authentication.Common.Validation;
using Microsoft.AspNetCore.Http;

namespace Facebook.Contracts.Authentication.Register;

public record RegisterRequest
{
	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	public string FirstName { get; init; }

	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	public string LastName { get; init; }

	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	[EmailAddress(ErrorMessage = "{PropertyValue} has wrong email format")]
	[StringLength(254, MinimumLength = 5, ErrorMessage = "{PropertyName} length must be between {2} and {1}.")]
	public string Email { get; init; }

	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	[StringLength(24, MinimumLength = 8, ErrorMessage = "{PropertyName} length must be between {2} and {1}.")]
	public string Password { get; init; }

	[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
	[StringLength(24, MinimumLength = 8, ErrorMessage = "{PropertyName} length must be between {2} and {1}.")]
	public string ConfirmPassword { get; init; }
	
	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	[DataType(DataType.Date, ErrorMessage = "Invalid date format")]
	public DateTime Birthday { get; init; }
	
	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	public string Gender { get; init; }
	
	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	[FileSize(2 * 1024 * 1024)]
	public required IFormFile Avatar { get; init; }
}



