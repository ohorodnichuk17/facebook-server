using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Admin.Command.UnBlockUser;

public class UnBlockUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UnBlockUserCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UnBlockUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userToUnBlock = await unitOfWork.Admin.UnblockUserAsync(request.UserId);

            if (userToUnBlock.IsError)
            {
                return userToUnBlock.Errors;
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            return Error.Failure(e.Message);
        }
    }
}