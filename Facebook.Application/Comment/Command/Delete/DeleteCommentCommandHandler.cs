using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.Delete;

public class DeleteCommentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommentCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Comment.DeleteAsync(request.Id);

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
