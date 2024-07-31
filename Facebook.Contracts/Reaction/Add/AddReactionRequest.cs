using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Reaction.Add;

public class AddReactionRequest
{
    [Required]
    public string TypeCode { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid PostId { get; set; }

}
