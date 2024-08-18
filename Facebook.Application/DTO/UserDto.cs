using Facebook.Domain.Post;
using Facebook.Domain.Story;

namespace Facebook.Application.DTO_s;

public class UserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Avatar { get; set; }
    public DateTime? Birthday { get; set; }
    public string Gender { get; set; }
    public List<StoryEntity> Stories { get; set; }
    public List<PostEntity> Posts { get; set; }
    public bool IsProfilePublic { get; set; }
}
