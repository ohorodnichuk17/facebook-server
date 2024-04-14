using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Authentication.Login;

public record LoginRequest
{
	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	[EmailAddress(ErrorMessage = "{PropertyValue} has wrong format")]
	[Length(5, 254)]
	public required string Email { get; init; }

	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	[Length(8, 24)]
	public required string Password { get; init; }
}
