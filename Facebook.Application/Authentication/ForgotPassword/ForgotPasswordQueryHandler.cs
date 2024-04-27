using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Application.Services;
using MediatR;

namespace Facebook.Application.Authentication.ForgotPassword;

public class ForgotPasswordQueryHandler 
    : IRequestHandler<ForgotPasswordQuery, ErrorOr<Success>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly EmailService _emailService;
    private readonly IJwtGenerator _jwtGenerator;

    public ForgotPasswordQueryHandler(IUserRepository userRepository, IUserAuthenticationService userAuthenticationService, EmailService emailService, IJwtGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _userAuthenticationService = userAuthenticationService;
        _emailService = emailService;
        _jwtGenerator = jwtGenerator;
    }

    // public ForgotPasswordQueryHandler(IUserRepository userRepository, IUserAuthenticationService userAuthenticationService, EmailService emailService)
    // {
    //     _userRepository = userRepository;
    //     _userAuthenticationService = userAuthenticationService;
    //     _emailService = emailService;
    // }

    
    public async Task<ErrorOr<Success>> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        var errorOrUser = await _userRepository.GetByEmailAsync(request.Email);
        if (errorOrUser.IsError)
        {
            return Error.Validation("User with this email doesn't exist");
        }

        var user = errorOrUser.Value;
        string token = await _userAuthenticationService.GeneratePasswordResetTokenAsync(user);
        string? userName;

        if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
        {
            if (string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.FirstName))
            {
                userName = user.Email;
            }
            else if (string.IsNullOrEmpty(user.LastName))
            {
                userName = user.FirstName;
            }
            else
            {
                userName = user.LastName;
            }
        }
        else
        {
            userName = user.FirstName + " " + user.LastName;
        }

        var sendEmailResult = await _emailService
            .SendResetPasswordEmailAsync(user.Email!, token, request.BaseUrl, userName);

        return sendEmailResult;
    }
}