using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Friends;

public record SearchUsersByFirstAndLastNamesRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string LastName { get; set; }
}