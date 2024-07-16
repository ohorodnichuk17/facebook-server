using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Action.Command.Add;

public class AddActionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddActionCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(AddActionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var action = new ActionEntity
            {
                Name = request.Name,
                Emoji = request.Emoji
            };

            var result = await unitOfWork.Action.CreateAsync(action);

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
