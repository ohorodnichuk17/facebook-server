using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.TypeExtensions;
using Facebook.Domain.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Facebook.Application.UserProfile.Command.Edit;

public class UserEditProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<UserEditProfileCommandHandler> logger,
    IImageStorageService imageStorageService)
    :
        IRequestHandler<UserEditProfileCommand, ErrorOr<UserProfileEntity>>
{
    public async Task<ErrorOr<UserProfileEntity>> Handle(UserEditProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting update user profile process...");

            var getProf = await unitOfWork.UserProfile.GetUserProfileByIdAsync(request.UserId);
            var getU = await userRepository.GetUserByIdAsync(request.UserId.ToString());
            if (!getProf.IsSuccess())
            {
                return Error.NotFound();
            }

            var userProfile = getProf.Value;
            var user = getU.Value;

            userProfile.Biography = request.Biography;
            userProfile.Country = request.Country;
            userProfile.Pronouns = request.Pronouns;
            userProfile.IsBlocked = request.isBlocked ?? userProfile.IsBlocked;;
            userProfile.IsProfilePublic = request.IsProfilePublic ?? userProfile.IsProfilePublic;;
            userProfile.Region = request.Region;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (request.CoverPhoto != null && request.CoverPhoto.Length != 0)
            {
                var coverPhoto = await imageStorageService.CoverPhotoAsync(userProfile, request.CoverPhoto);
                userProfile.CoverPhoto = coverPhoto;
            }
            if (request.Avatar != null && request.Avatar.Length != 0)
            {
                var avatar = await imageStorageService.AddAvatarAsync(user, request.Avatar);
                user.Avatar = avatar;
            }

            var editprofileResult = await unitOfWork.UserProfile.UserEditProfileAsync(userProfile, user.FirstName, user.LastName, user.Avatar);

            if (editprofileResult.IsError)
                return editprofileResult.Errors;

            logger.LogInformation("User update profile process completed successfully");

            return editprofileResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user registration");
            throw;
        }
    }
}
