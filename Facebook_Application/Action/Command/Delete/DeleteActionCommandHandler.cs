using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Action.Command.Delete;

public class DeleteActionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteActionCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Action.DeleteAsync(request.Id);

            if (result.IsError)
            {
                return result.Errors;
            }

            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
