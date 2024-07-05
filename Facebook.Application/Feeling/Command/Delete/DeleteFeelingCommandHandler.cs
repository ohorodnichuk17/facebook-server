using ErrorOr;
using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using MediatR;

namespace Facebook.Application.Feeling.Command.Delete;

public class DeleteFeelingCommandHandler(IFeelingRepository repository)
    : IRequestHandler<DeleteFeelingCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteFeelingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.DeleteFeelingAsync(request.Id);

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