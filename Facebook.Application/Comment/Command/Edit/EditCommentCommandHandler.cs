using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.Edit;

public class EditCommentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<EditCommentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(EditCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var getComment = await unitOfWork.Comment.GetByIdAsync(request.Id);
            if (getComment.IsError)
            {
                return Error.NotFound();
            }

            var comment = getComment.Value;

            if (request.Message != null)
            {
                comment.Message = request.Message;
            }

            var editComment = await unitOfWork.Comment.UpdateCommentAsync(comment);

            if (editComment.IsError)
            {
                return editComment.Errors;
            }


            return Unit.Value;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
