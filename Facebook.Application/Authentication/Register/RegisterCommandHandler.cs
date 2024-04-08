using MediatR;
using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Authentication.SendConfirmationEmail;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.User;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.TypeExtensions;
using Microsoft.Extensions.Logging;


namespace Facebook.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : 
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISender _mediatr;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(IUserRepository userRepository, ISender mediatr, ILogger<RegisterCommandHandler> logger, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _mediatr = mediatr;
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
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

            var user = new User
            {
                FirstName = command.FirstName, 
                LastName = command.LastName,
                Email = command.Email, 
                PasswordHash = command.Password,
                Birthday = command.Birthday, 
                Gender = command.Gender
            };

            var role = Roles.User;

            var userResult = await _userRepository.CreateUserAsync(user, command.Password, role);

            if (userResult.IsError)
            {
                return userResult.Errors;
            }

            var sendConfirmationResult = await _mediatr.Send(
                new SendConfirmationEmailCommand(user.Email, command.BaseUrl));

            if (sendConfirmationResult.IsError)
            {
                return sendConfirmationResult.Errors;
            }

            var token = _jwtTokenGenerator.GenerateJwtTokenAsync(user, role);

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