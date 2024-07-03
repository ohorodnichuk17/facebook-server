using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Domain.User;

public class UserEntity : IdentityUser<Guid>
{
   public string FirstName { get; set; }
   public string LastName { get; set; }

   private DateTime _birthday;
   public DateTime Birthday
   {
      get { return _birthday; }
      set
      {
         _birthday = DateTime.SpecifyKind(value, DateTimeKind.Utc);
      }
   }

   public string? Avatar { get; set; }

   public string Gender { get; set; }

   public ICollection<StoryEntity> Stories { get; set; } = new List<StoryEntity>();
   public ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();

   public ICollection<FriendRequestEntity> SentFriendRequests { get; set; } = new List<FriendRequestEntity>();
   public ICollection<FriendRequestEntity> ReceivedFriendRequests { get; set; } = new List<FriendRequestEntity>();
}