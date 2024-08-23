using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Action.Add;

public record AddActionRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string Name { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string Emoji { get; set; }
}
