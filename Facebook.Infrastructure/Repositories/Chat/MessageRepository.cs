using ErrorOr;
using Facebook.Application.Common.Interfaces.Chat.IRepository;
using Facebook.Domain.Chat;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Infrastructure.Repositories.Chat;

public class MessageRepository(FacebookDbContext context) : Repository<MessageEntity>(context), IMessageRepository
{
    public async Task<ErrorOr<IEnumerable<MessageEntity>>> GetMessagesByChatIdAsync(Guid chatId)
    {
        return await context.Messages
                .Where(m => m.ChatId == chatId)
                .ToListAsync();
    }
}
