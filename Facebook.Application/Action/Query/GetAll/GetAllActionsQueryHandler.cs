using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Action.Query.GetAll;

public class GetAllActionsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllActionsQuery, ErrorOr<IEnumerable<ActionEntity>>>
{
    public async Task<ErrorOr<IEnumerable<ActionEntity>>> Handle(GetAllActionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Action.GetAllAsync();

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
            Console.WriteLine($"Error while receiving actions: {ex.Message}");
            return Error.Failure($"Error while receiving actions: {ex.Message}");
        }
    }
}
