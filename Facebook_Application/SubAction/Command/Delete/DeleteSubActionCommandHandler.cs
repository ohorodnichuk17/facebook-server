using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.SubAction.Command.Delete;

public class DeleteSubActionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteSubActionCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteSubActionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.SubAction.DeleteAsync(request.Id);

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
