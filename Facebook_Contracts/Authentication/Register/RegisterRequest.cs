using Microsoft.AspNetCore.Http;

namespace Facebook.Contracts.Authentication.Register;

public record RegisterRequest
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string Password { get; init; }

    public string ConfirmPassword { get; init; }

    public DateTime Birthday { get; init; }

    public string Gender { get; init; }
    
    public IFormFile? Avatar { get; init; }
    public string? Role { get; init; }
}



