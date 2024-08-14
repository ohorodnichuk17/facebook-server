using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.UserProfile.Common;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Facebook.Application.UserProfile.Command.Edit;

public class UserEditProfileCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<UserEditProfileCommandHandler> logger,
    IJwtGenerator jwtGenerator,
    IMapper mapper,
    IImageStorageService imageStorageService)
    :
        IRequestHandler<UserEditProfileCommand, ErrorOr<EditProfileResult>>
{
    public async Task<ErrorOr<EditProfileResult>> Handle(UserEditProfileCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting update user profile process...");

            var getProf = await unitOfWork.UserProfile.GetUserProfileByIdAsync(request.UserId);
            var getU = await unitOfWork.User.GetUserByIdAsync(request.UserId.ToString());
            if (!getProf.IsSuccess() || !getU.IsSuccess())
            {
                return Error.NotFound();
            }

            var userProfile = getProf.Value;
            var user = getU.Value;

            userProfile.Biography = request.Biography ?? userProfile.Biography;
            userProfile.Country = request.Country ?? userProfile.Country;
            userProfile.Pronouns = request.Pronouns ?? userProfile.Pronouns;
            userProfile.IsProfilePublic = request.IsProfilePublic ?? userProfile.IsProfilePublic;
            userProfile.Region = request.Region ?? userProfile.Region;

            user.IsOnline = request.isOnline ?? user.IsOnline;

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                user.FirstName = request.FirstName;
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                user.LastName = request.LastName;
            }

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

            var editprofileResult =
                await unitOfWork.UserProfile.UserEditProfileAsync(userProfile, user.FirstName, user.LastName,
                    user.Avatar, user.IsOnline);

            if (editprofileResult.IsError)
            {
                return editprofileResult.Errors;
            }

            logger.LogInformation("User update profile process completed successfully");

            var token = await jwtGenerator.GenerateJwtTokenAsync(user, Roles.User);

            var userProfileValue = editprofileResult.Value;

            var result = mapper.Map<EditProfileResult>((userProfileValue, token));

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user registration");
            throw;
        }
    }
}
