using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Query.GetById;

public class GetReactionByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetReactionByIdQuery, ErrorOr<ReactionEntity>>
{
    public async Task<ErrorOr<ReactionEntity>> Handle(GetReactionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Reaction.GetByIdAsync(request.Id);

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
            Console.WriteLine($"Error retrieving reaction by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving reaction by id {request.Id}: {ex.Message}");
        }
    }
}
