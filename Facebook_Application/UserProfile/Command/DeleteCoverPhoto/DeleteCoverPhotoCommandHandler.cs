using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.UserProfile.Command.DeleteCoverPhoto;

public class DeleteCoverPhotoCommandHandler(IUnitOfWork unitOfWork,
    IImageStorageService imageStorageService)
    : IRequestHandler<DeleteCoverPhotoCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteCoverPhotoCommand request, CancellationToken cancellationToken)
    {
        var userProfileResult = await unitOfWork.UserProfile
            .GetUserProfileByIdAsync(request.UserId);
        if (userProfileResult.IsError)
        {
            return userProfileResult.Errors;
        }

        var userProfile = userProfileResult.Value;

        if (!string.IsNullOrEmpty(userProfile.CoverPhoto))
        {
            var deletionResult = await imageStorageService.DeleteImageAsync(userProfile.CoverPhoto);
            if (!deletionResult)
            {
                return Error.Failure("Failed to delete cover photo");
            }

            userProfile.CoverPhoto = null;
            await unitOfWork.UserProfile.UserEditProfileAsync(userProfile);

            return true;
        }
        return Error.Failure("User has no cover photo to delete.");
    }
}