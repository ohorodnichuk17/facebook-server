using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.TypeExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Command.Delete;

public class DeleteMessageByIdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageByIdCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteMessageByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = await unitOfWork.Message.DeleteAsync(request.Id);
            if (message.IsSuccess())
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
