namespace Facebook.Domain.Post;

public class SubActionEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ActionEntity Action { get; set; }
    public Guid ActionId { get; set; }
}
