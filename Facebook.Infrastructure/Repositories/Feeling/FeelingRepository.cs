using ErrorOr;
using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Feeling;

public class FeelingRepository(FacebookDbContext context) : IFeelingRepository
{
    public async Task<ErrorOr<IEnumerable<FeelingEntity>>> GetAllFeelingsAsync()
    {
        return await context.Feelings.ToListAsync();
    }

    public async Task<ErrorOr<FeelingEntity>> GetFeelingById(Guid id)
    {
        var feeling = await context.Feelings.FindAsync(id);

        if (feeling == null)
        {
            return Error.Failure("Feeling not found");
        }

        return feeling;
    }

    public async Task<ErrorOr<Guid>> AddFeelingAsync(FeelingEntity feeling)
    {
        try
        {
            context.Feelings.Add(feeling);
            await context.SaveChangesAsync();
            return feeling.Id;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteFeelingAsync(Guid feelingId)
    {
        try
        {
            var feeling = await context.Feelings.FindAsync(feelingId);

            if (feeling == null)
            {
                return Error.Failure("Story not found");
            }

            context.Feelings.Remove(feeling);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> SaveFeelingAsync(FeelingEntity feeling)
    {
        await context.SaveChangesAsync();
        return Unit.Value;
    }
}