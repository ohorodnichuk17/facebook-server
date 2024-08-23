using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.UserProfile.Common;
using Facebook.Domain.Constants.Roles;
using MediatR;

namespace Facebook.Application.UserProfile.Command.DeleteAvatar;

public class DeleteAvatarCommandHandler(IUnitOfWork unitOfWork,
    IImageStorageService imageStorageService,
    IJwtGenerator jwtGenerator)
    : IRequestHandler<DeleteAvatarCommand, ErrorOr<DeleteAvatarResult>>
{
    public async Task<ErrorOr<DeleteAvatarResult>> Handle(DeleteAvatarCommand request, CancellationToken cancellationToken)
    {
        var userResult = await unitOfWork.User.GetUserByIdAsync(request.UserId);

        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        var user = userResult.Value;

        if (!string.IsNullOrEmpty(user.Avatar))
        {
            var deletionResult = await imageStorageService.DeleteImageAsync(user.Avatar);
            if (!deletionResult)
            {
                return Error.Failure("Failed to delete avatar");
            }

            user.Avatar = null;
            await unitOfWork.User.UpdateAsync(user);

            var token = await jwtGenerator.GenerateJwtTokenAsync(user, Roles.User);

            return new DeleteAvatarResult(token);
        }

        return Error.Failure("User has no avatar to delete.");
    }
}