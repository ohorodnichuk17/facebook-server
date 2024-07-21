using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Admin.Command.BlockUser;

public class BlockUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<BlockUserCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userToBlock = await unitOfWork.Admin.BlockUserAsync(request.UserId);

            if (userToBlock.IsError)
            {
                return userToBlock.Errors;
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            return Error.Failure(e.Message);
        }
    }
}