namespace Facebook.Contracts.Authentication.Common.Response;

public record AuthenticationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Birthday,
    string Gender,
    string Token);