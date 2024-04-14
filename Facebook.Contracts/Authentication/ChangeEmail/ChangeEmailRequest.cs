namespace Facebook.Contracts.Authentication.ChangeEmail;

public record ChangeEmailRequest
{
    //[Required(ErrorMessage = "{PropertyName} must not be empty")]
    //[EmailAddress(ErrorMessage = "{PropertyValue} has wrong format")]
    //[Length(5, 254)]
    public required string Email { get; init; }
    
    //[Required(ErrorMessage = "{PropertyName} must not be empty")]
    //[Length(256, 4096)]
    public required string Token { get; init; }
    
    //[Required(ErrorMessage = "{PropertyName} must not be empty")]
    //[Length(36,36)]
    public required string UserId { get; init; }
}