using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Query.GetAll;

public class GetAllReactionsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllReactionsQuery, ErrorOr<IEnumerable<ReactionEntity>>>
{
    public async Task<ErrorOr<IEnumerable<ReactionEntity>>> Handle(GetAllReactionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Reaction.GetAllAsync();

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
            Console.WriteLine($"Error while receiving reactions: {ex.Message}");
            return Error.Failure($"Error while receiving reactions: {ex.Message}");
        }
    }
}
