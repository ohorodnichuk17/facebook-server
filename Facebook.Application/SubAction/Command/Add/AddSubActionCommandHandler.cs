using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.SubAction.Command.Add;

public class AddSubActionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddSubActionCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(AddSubActionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var subAction = new SubActionEntity
            {
                Name = request.Name,
                ActionId = request.ActionId,
            };

            var result = await unitOfWork.SubAction.CreateAsync(subAction);

            if (result.IsError)
            {
                return result.Errors;
            }

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
