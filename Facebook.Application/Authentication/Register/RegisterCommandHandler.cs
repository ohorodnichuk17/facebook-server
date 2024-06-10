using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Authentication.SendConfirmationEmail;
using Facebook.Application.Common.Interfaces.Admin;
using Facebook.Application.Common.Interfaces.Admin.IRepository;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.TypeExtensions;
using Facebook.Domain.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Facebook.Application.Authentication.Register;

public class RegisterCommandHandler(
    IAdminRepository adminRepository,
    IUserRepository userRepository,
    IUserProfileRepository userProfileRepository,
    ISender mediatr,
    ILogger<RegisterCommandHandler> logger,
    IJwtGenerator jwtGenerator,
    IImageStorageService imageStorageService)
    :
        IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command,
    CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting user registration process...");

            var errorOrUser = await userRepository.GetByEmailAsync(command.Email);

            if (errorOrUser.IsSuccess())
            {
                logger.LogWarning("User with email {Email} already exists", command.Email);
                return Error.Validation("User with this email already exists");
            }

            var user = new UserEntity
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PasswordHash = command.Password,
                Birthday = command.Birthday,
                Gender = command.Gender,
            };

            var role = Roles.User;

            var userResult = await adminRepository.CreateAsync(user, command.Password, role);
            var userProfileResult = await userProfileRepository.UserCreateProfileAsync(user.Id);

            if (userResult.IsError || userProfileResult.IsError)
            {
                return userResult.Errors;
            }

            if (command.Avatar != null)
            {
                var imageName = await imageStorageService.AddAvatarAsync(user, command.Avatar);
                if (imageName == null)
                {
                    return Error.Unexpected("Avatar saving error");
                }
                user.Avatar = imageName;
            }

            var avatarResult = await userRepository.SaveUserAsync(user);
            if (avatarResult.IsError)
            {
                return avatarResult.Errors;
            }

            var sendConfirmationResult = await mediatr.Send(
                new SendConfirmationEmailCommand(user.Email, command.BaseUrl));

            if (sendConfirmationResult.IsError)
            {
                return sendConfirmationResult.Errors;
            }

            var token = await jwtGenerator.GenerateJwtTokenAsync(user, role);

            logger.LogInformation("User registration process completed successfully");

            return new AuthenticationResult(user.Id, user, token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user registration");
            throw;
        }
    }


}