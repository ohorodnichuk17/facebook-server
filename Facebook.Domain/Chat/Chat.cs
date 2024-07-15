using Facebook.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Domain.Chat;

public class ChatEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<MessageEntity> Messages { get; set; }
    public ICollection<UserEntity> Users { get; set; }
    public ICollection<ChatUserEntity> ChatUsers { get; set; }
}
