using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Reaction.Add;

public class AddReactionRequest
{
    [Required]
    public required string Emoji { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid PostId { get; set; }
}
