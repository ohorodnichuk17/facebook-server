using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Feeling.Command.Delete;

public class DeleteFeelingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteFeelingCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteFeelingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Feeling.DeleteAsync(request.Id);

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