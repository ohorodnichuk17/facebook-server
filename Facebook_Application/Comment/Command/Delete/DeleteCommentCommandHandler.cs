using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Comment.Command.Delete;

public class DeleteCommentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommentCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Comment.DeleteAsync(request.Id);

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
