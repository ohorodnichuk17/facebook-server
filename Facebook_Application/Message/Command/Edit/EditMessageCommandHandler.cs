using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Command.Edit;

public class EditMessageCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<EditMessageCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(EditMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var getMessage = await unitOfWork.Message.GetByIdAsync(request.Id);
            if (getMessage.IsError)
            {
                return Error.NotFound();
            }

            var message = getMessage.Value;

            if (request.Content != null)
            {
                message.Content = request.Content;
            }

            var editMessage = await unitOfWork.Message.UpdateMessageAsync(message);

            if (editMessage.IsError)
            {
                return editMessage.Errors;
            }


            return Unit.Value;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
