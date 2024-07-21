using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Chat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Chat.Query.GetChatsByUserId;

public class GetChatsByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetChatsByUserIdQuery, ErrorOr<IEnumerable<ChatEntity>>>
{
    public async Task<ErrorOr<IEnumerable<ChatEntity>>> Handle(GetChatsByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Chat.GetChatsByUserIdAsync(request.UserId);

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
            Console.WriteLine($"Error retrieving chats by user id {request.UserId}: {ex.Message}");
            return Error.Failure($"Error retrieving chats by user id {request.UserId}: {ex.Message}");
        }
    }
}
