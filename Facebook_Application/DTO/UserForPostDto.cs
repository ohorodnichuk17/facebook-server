namespace Facebook.Application.DTO;

public class UserForPostDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public string? LastName { get; set; }
    public DateTime Birthday { get; set; }
    public string? Avatar { get; set; }
    public string Gender { get; set; }
    public bool IsOnline { get; set; }
}
