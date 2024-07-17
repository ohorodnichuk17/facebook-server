using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Authentication.ConfirmEmail;

public class ConfirmEmailCommandHandler(
    IUserAuthenticationService userAuthenticationService,
    IJwtGenerator jwtGenerator,
    IUnitOfWork unitOfWork)
    :
        IRequestHandler<ConfirmEmailCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var errorOrSuccess = await userAuthenticationService
            .ConfirmEmailAsync(request.UserId, request.ValidEmailToken);
        if (errorOrSuccess.IsError)
        {
            return errorOrSuccess.Errors;
        }

        var userOrError = await unitOfWork.User.GetUserByIdAsync(request.UserId.ToString());
        if (userOrError.IsError)
        {
            return userOrError.Errors;
        }

        var user = userOrError.Value;
        var roleOrError = await unitOfWork.User.FindRolesByUserIdAsync(user);
        if (roleOrError.IsError)
        {
            return roleOrError.Errors;
        }

        var role = roleOrError.Value.First();
        var token = await jwtGenerator.GenerateJwtTokenAsync(user, role);

        return token;
    }
}
