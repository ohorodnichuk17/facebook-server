namespace Facebook.Contracts.Authentication.ChangeEmail;

public record ChangeEmailRequest
{
    public required string Email { get; init; }
    
    public required string Token { get; init; }
    
    public required string UserId { get; init; }
}