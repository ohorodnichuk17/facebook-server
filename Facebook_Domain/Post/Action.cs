namespace Facebook.Domain.Post;

public class ActionEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Emoji { get; set; }
    public ICollection<SubActionEntity> SubActions { get; set; }
}
