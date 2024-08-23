using ErrorOr;
using Facebook.Application.Admin.Command.BanUser;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Admin.Command.UnBanUser;

public class UnBanUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UnBanUserCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UnBanUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userToUnBan = await unitOfWork.Admin.UnbanUserAsync(request.UserId);

            if (userToUnBan.IsError)
            {
                return userToUnBan.Errors;
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            return Error.Failure(e.Message);
        }
    }
}