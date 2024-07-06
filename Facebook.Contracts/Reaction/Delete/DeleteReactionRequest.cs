namespace Facebook.Contracts.Reaction.Delete;

public class DeleteReactionRequest
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
}
