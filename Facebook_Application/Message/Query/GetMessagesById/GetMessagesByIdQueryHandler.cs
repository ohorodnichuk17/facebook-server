using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Chat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Query.GetMessagesById;

public class GetMessagesByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMessagesByIdQuery, ErrorOr<IEnumerable<MessageEntity>>>
{
    public async Task<ErrorOr<IEnumerable<MessageEntity>>> Handle(GetMessagesByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Message.GetMessagesByChatIdAsync(request.ChatId);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving messages by chat id {request.ChatId}: {ex.Message}");
            return Error.Failure($"Error retrieving messages by chat id {request.ChatId}: {ex.Message}");
        }
    }
}
