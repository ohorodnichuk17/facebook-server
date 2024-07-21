using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.TypeExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Chat.Command.Delete;

public class DeleteChatByIdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteChatByIdCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteChatByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var chat = await unitOfWork.Chat.DeleteAsync(request.Id);
            if (chat.IsSuccess())
            {
                return true;
            }
            else
            {
                return Error.NotFound();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
