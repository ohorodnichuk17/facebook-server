using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Message.Edit;

public class EditMessageRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid Id { get; set; }
    [Required]
    public string Content { get; set; }
}
