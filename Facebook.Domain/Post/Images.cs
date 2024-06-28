namespace Facebook.Domain.Post;

public class ImagesEntity
{
    public Guid Id { get; set; }
    public int PriorityImage { get; set; }
    public string ImagePath { get; set; }

    public Guid PostId { get; set; }
    public PostEntity? Post { get; set; }
}