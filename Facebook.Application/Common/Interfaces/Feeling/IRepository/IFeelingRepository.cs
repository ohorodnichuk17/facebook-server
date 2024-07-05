using Facebook.Domain.Post;
using ErrorOr;
using MediatR;

namespace Facebook.Application.Common.Interfaces.Feeling.IRepository;

public interface IFeelingRepository
{
    Task<ErrorOr<IEnumerable<FeelingEntity>>> GetAllFeelingsAsync();
    Task<ErrorOr<FeelingEntity>> GetFeelingById(Guid id);
    Task<ErrorOr<Guid>> AddFeelingAsync(FeelingEntity feeling);
    Task<ErrorOr<bool>> DeleteFeelingAsync(Guid feelingId);

    Task<ErrorOr<Unit>> SaveFeelingAsync(FeelingEntity feeling);
}