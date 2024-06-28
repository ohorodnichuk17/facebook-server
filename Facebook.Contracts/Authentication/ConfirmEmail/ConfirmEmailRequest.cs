using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Authentication.ConfirmEmail;

public record ConfirmEmailRequest {

	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	public required Guid UserId { get; init; }

	[Required(ErrorMessage = "{PropertyName} must not be empty")]
	[Length(256, 4096)]
	public required string ValidEmailToken { get; init; }
}
 
