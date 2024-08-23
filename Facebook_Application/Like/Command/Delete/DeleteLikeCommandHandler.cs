using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Like.Command.Delete;

public class DeleteLikeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteLikeCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Like.DeleteAsync(request.Id);

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
