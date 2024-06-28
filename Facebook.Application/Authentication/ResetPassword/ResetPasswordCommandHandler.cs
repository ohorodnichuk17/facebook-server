using MediatR;
using ErrorOr;
using Facebook.Application.Services;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Application.Authentication.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<UserEntity>>
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EmailService _emailService;

        public ResetPasswordCommandHandler(UserManager<UserEntity> userManager, EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ErrorOr<UserEntity>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Error.Validation("User with this email doesn't exist");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);
            if (!result.Succeeded)
            {
                return Error.Validation("Failed to reset password");
            }

            var resetPasswordEmailResult = await _emailService.SendResetPasswordEmailAsync(request.Email, resetToken, request.BaseUrl, user.UserName);


            return user;
        }
    }
}