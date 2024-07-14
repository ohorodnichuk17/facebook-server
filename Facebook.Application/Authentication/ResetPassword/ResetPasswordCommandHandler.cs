using ErrorOr;
using Facebook.Application.Services;
using Facebook.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Application.Authentication.ResetPassword
{
    public class ResetPasswordCommandHandler(UserManager<UserEntity> userManager, EmailService emailService)
        : IRequestHandler<ResetPasswordCommand, ErrorOr<UserEntity>>
    {
        public async Task<ErrorOr<UserEntity>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Error.Validation("User with this email doesn't exist");
            }
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, resetToken, request.Password);
            if (!result.Succeeded)
            {
                return Error.Validation("Failed to reset password");
            }

            var resetPasswordEmailResult = await emailService.SendResetPasswordEmailAsync(request.Email, resetToken, request.BaseUrl, user.UserName);


            return user;
        }
    }
}