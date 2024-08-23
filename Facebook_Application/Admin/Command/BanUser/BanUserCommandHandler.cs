using ErrorOr;
using Facebook.Application.Admin.Command.BlockUser;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Admin.Command.BanUser;

public class BanUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<BanUserCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userToBan = await unitOfWork.Admin.BanUserAsync(request.UserId);

            if (userToBan.IsError)
            {
                return userToBan.Errors;
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            return Error.Failure(e.Message);
        }
    }
}