using Facebook.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Domain.Chat;

public class UserChatEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid ChatId { get; set; }
    public ChatEntity Chat { get; set; }
}
