using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Action.Query.GetById;

public class GetActionByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetActionByIdQuery, ErrorOr<ActionEntity>>
{
    public async Task<ErrorOr<ActionEntity>> Handle(GetActionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Action.GetByIdAsync(request.Id);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving action by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving action by id {request.Id}: {ex.Message}");
        }
    }
}
