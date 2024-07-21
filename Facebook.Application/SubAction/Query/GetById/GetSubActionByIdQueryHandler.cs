using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.SubAction.Query.GetById;

public class GetSubActionByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSubActionByIdQuery, ErrorOr<SubActionEntity>>
{
    public async Task<ErrorOr<SubActionEntity>> Handle(GetSubActionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.SubAction.GetByIdAsync(request.Id);

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
            Console.WriteLine($"Error retrieving subAction by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving subAction by id {request.Id}: {ex.Message}");
        }
    }
}
