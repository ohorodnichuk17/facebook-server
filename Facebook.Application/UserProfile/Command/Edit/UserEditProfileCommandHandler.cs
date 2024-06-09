using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.TypeExtensions;
using Facebook.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Command.Edit;

public class UserEditProfileCommandHandler :
 IRequestHandler<UserEditProfileCommand, ErrorOr<UserProfileEntity>>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserEditProfileCommandHandler> _logger;
    private readonly IImageStorageService _imageStorageService;

    public UserEditProfileCommandHandler(IUserRepository userRepository, IUserProfileRepository userProfileRepository, ILogger<UserEditProfileCommandHandler> logger, IImageStorageService imageStorageService)
    {
        _userProfileRepository = userProfileRepository;
        _logger = logger;
        _imageStorageService = imageStorageService;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<UserProfileEntity>> Handle(UserEditProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting update user profile process...");

            var getProf = await _userProfileRepository.GetUserProfileByIdAsync(request.UserId.ToString());
            var getU = await _userRepository.GetUserByIdAsync(request.UserId.ToString());
            if (!getProf.IsSuccess())
            {
                return Error.NotFound();
            }

            var userProfile = getProf.Value;
            var user = getU.Value;

            userProfile.Biography = request.Biography;
            userProfile.City = request.City;
            userProfile.Country = request.Country;
            userProfile.Pronouns = request.Pronouns;
            userProfile.IsBlocked = request.isBlocked;
            userProfile.IsProfilePublic = request.IsProfilePublic;
            userProfile.Region = request.Region;
            user.UserName = request.UserName;
            if (request.CoverPhoto != null && request.CoverPhoto.Length != 0)
            {
                var coverPhoto = await _imageStorageService.CoverPhotoAsync(userProfile, request.CoverPhoto);
                userProfile.CoverPhoto = coverPhoto;
            }
            if (request.Avatar != null && request.Avatar.Length != 0)
            {
                var avatar = await _imageStorageService.AddAvatarAsync(user, request.Avatar);
                user.Avatar = avatar;
            }

            var editprofileResult = await _userProfileRepository.UserEditProfileAsync(userProfile, user.UserName, user.Avatar);

            if (editprofileResult.IsError)
                return editprofileResult.Errors;

            _logger.LogInformation("User update profile process completed successfully");

            return editprofileResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user registration");
            throw;
        }
    }
}
