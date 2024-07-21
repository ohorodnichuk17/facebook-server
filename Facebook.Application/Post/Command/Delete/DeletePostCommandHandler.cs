using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Post.Command.Delete;

public class DeletePostCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePostCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Post.DeleteAsync(request.Id);

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