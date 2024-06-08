using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Authentication.SendConfirmationEmail;
using Facebook.Application.Common.Interfaces.Admin;
using Facebook.Application.Common.Interfaces.Admin.IRepository;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.TypeExtensions;
using Facebook.Domain.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Facebook.Application.Authentication.Register;

public class RegisterCommandHandler : 
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISender _mediatr;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IImageStorageService _imageStorageService;

    public RegisterCommandHandler(
        IAdminRepository adminRepository, 
        IUserRepository userRepository,
        ISender mediatr,
        ILogger<RegisterCommandHandler> logger, 
        IJwtGenerator jwtGenerator,
        IImageStorageService imageStorageService)
    {
        _adminRepository = adminRepository;
        _userRepository = userRepository;
        _mediatr = mediatr;
        _logger = logger;
        _jwtGenerator = jwtGenerator;
        _imageStorageService = imageStorageService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, 
    CancellationToken cancellationToken)
{
    try
    {
        _logger.LogInformation("Starting user registration process...");
    
        var errorOrUser = await _userRepository.GetByEmailAsync(command.Email);

        if (errorOrUser.IsSuccess())
        {
            _logger.LogWarning("User with email {Email} already exists", command.Email);
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

        var userResult = await _adminRepository.CreateAsync(user, command.Password, role);

        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        if (command.Avatar != null) 
        {
            var imageName = await _imageStorageService.AddAvatarAsync(user, command.Avatar);
            if (imageName == null)
            {
                return Error.Unexpected("Avatar saving error");
            }
            user.Avatar = imageName;
        }


        var avatarResult = await _userRepository.SaveUserAsync(user);
        if (avatarResult.IsError)
        {
            return avatarResult.Errors;
        }

        var sendConfirmationResult = await _mediatr.Send(
            new SendConfirmationEmailCommand(user.Email, command.BaseUrl));

        if (sendConfirmationResult.IsError)
        {
            return sendConfirmationResult.Errors;
        }

        var token = await _jwtGenerator.GenerateJwtTokenAsync(user, role);

        _logger.LogInformation("User registration process completed successfully");

        return new AuthenticationResult(user.Id, user, token);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred during user registration");
        throw;
    }
}


}