using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Domain.TypeExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Command.Delete;

public class DeleteReactionCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteReactionCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reaction = await unitOfWork.Reaction.DeleteAsync(request.Id);
            if (reaction.IsSuccess())
            {
                return true;
            }
            else
            {
                return Error.NotFound();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
