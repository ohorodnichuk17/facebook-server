using ErrorOr;
using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Feeling.Query.GetById;

public class GetFeelingByIdQueryHandler(IFeelingRepository feelingRepository)
    : IRequestHandler<GetFeelingByIdQuery, ErrorOr<FeelingEntity>>
{
    public async Task<ErrorOr<FeelingEntity>> Handle(GetFeelingByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await feelingRepository.GetFeelingById(request.Id);

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
            Console.WriteLine($"Error retrieving feeling by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving feeling by id {request.Id}: {ex.Message}");
        }
    }
}