using ErrorOr;
using Facebook.Domain.Chat;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.Application.Common.Interfaces.IRepository.Chat;

namespace Facebook.Infrastructure.Repositories.Chat;

public class MessageRepository(FacebookDbContext context) : Repository<MessageEntity>(context), IMessageRepository
{
    public async Task<ErrorOr<IEnumerable<MessageEntity>>> GetMessagesByChatIdAsync(Guid chatId)
    {
        return await context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
    }

    public async Task<ErrorOr<MediatR.Unit>> UpdateMessageAsync(MessageEntity message)
    {
        try
        {
            var messageExist = await context.Messages.FindAsync(message.Id);

            if (messageExist == null)
            {
                return Error.Failure("Message not found");
            }

            messageExist.Content = message.Content;
            
            context.Messages.Update(messageExist);
            await context.SaveChangesAsync();

            return MediatR.Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
