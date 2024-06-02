using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Domain.Post;

public class ImagesEntity
{
    [Key]
    public Guid Id { get; set; }
    public int PriorityImage { get; set; }
    public string ImagePath { get; set; } 
    
    [ForeignKey("PostEntity")]
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
}