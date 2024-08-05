using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Authentication.SendConfirmationEmail;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IRepository.Admin;
using Facebook.Application.Common.Interfaces.IRepository.User;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.TypeExtensions;
using Facebook.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Facebook.Application.Authentication.Register;

public class RegisterCommandHandler(
    IAdminRepository adminRepository,
    IUnitOfWork unitOfWork,
    IUserProfileRepository userProfileRepository,
    ISender mediatr,
    ILogger<RegisterCommandHandler> logger,
    IJwtGenerator jwtGenerator,
    IImageStorageService imageStorageService,
    ICurrentUserService currentUserService,
    UserManager<UserEntity> userManager)
    :
        IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command,
    CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting user registration process...");

            var errorOrUser = await unitOfWork.User.GetByEmailAsync(command.Email);

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
                EmailConfirmed = command.Role?.ToLower() == Roles.Admin
            };

            var currentUserRole = currentUserService.GetCurrentUserRole();
            
            var role = command.Role ?? Roles.User; 

            if (currentUserRole == Roles.Admin && command.Role != null)
            {
                role = command.Role == Roles.Admin ? Roles.Admin : Roles.User;
            }

            var userResult = await adminRepository.CreateAsync(user, command.Password, role);
            var userProfileResult = await userProfileRepository.UserCreateProfileAsync(user.Id);


            int indexOfAt = command.Email.IndexOf("@");

            string userName = command.Email.Substring(0, indexOfAt);

            await userManager.SetUserNameAsync(user, userName);

            if (userResult.IsError || userProfileResult.IsError)
            {
                return userResult.Errors;
            }

            if (command.Avatar != null && command.Avatar.Length > 0)
            {
                var imageName = await imageStorageService.AddAvatarAsync(user, command.Avatar);
                if (imageName == null)
                {
                    return Error.Unexpected("Avatar saving error");
                }
                user.Avatar = imageName;
            }

            var avatarResult = await unitOfWork.User.SaveUserAsync(user);
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