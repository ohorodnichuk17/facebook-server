using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.SubAction.Query.GetAll;

public class GetAllSubActionsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllSubActionsQuery, ErrorOr<IEnumerable<SubActionEntity>>>
{
    public async Task<ErrorOr<IEnumerable<SubActionEntity>>> Handle(GetAllSubActionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.SubAction.GetAllAsync();

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
            Console.WriteLine($"Error while receiving subActions: {ex.Message}");
            return Error.Failure($"Error while receiving subActions: {ex.Message}");
        }
    }
}
