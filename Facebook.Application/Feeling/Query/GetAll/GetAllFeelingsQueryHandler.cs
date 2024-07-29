using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Feeling.Query.GetAll;

public class GetAllFeelingsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllFeelingsQuery, ErrorOr<IEnumerable<FeelingEntity>>>
{
    public async Task<ErrorOr<IEnumerable<FeelingEntity>>> Handle(GetAllFeelingsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Feeling.GetAllAsync();

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
            Console.WriteLine($"Error while receiving feelings: {ex.Message}");
            return Error.Failure($"Error while receiving feelings: {ex.Message}");
        }
    }
}