using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Feeling.Add;

public record AddFeelingRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string Name { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string Emoji { get; set; }
    
}