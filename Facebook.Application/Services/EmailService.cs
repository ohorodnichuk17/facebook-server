using System.Text;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.WebUtilities;

namespace Facebook.Application.Services;

public class EmailService(ISmtpService smtpService)
{
    public async Task<ErrorOr<Success>> SendEmailConfirmationEmailAsync(
        Guid userId, string email, string token, string baseUrl)
    {
        var encodedEmailToken = Encoding.UTF8.GetBytes(token);

        var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

        //ToDo make EmailBody
        string url = $"{baseUrl}/authentication/confirm-email?userid={userId}&token={validEmailToken}";

        string emailBody = $"<h1>Confirm your email</h1> <a href='{url}'>Confirm now</a>";

        await smtpService.SendEmailAsync(email, "Email confirmation.", emailBody);

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> SendResetPasswordEmail(string email, string baseUrl, string token)
    {
        var encodedToken = Encoding.UTF8.GetBytes(token);

        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        string url = $"{baseUrl}/authentication/reset-password?email={email}&token={validToken}";

        //ToDo make EmailBody
        string emailBody = "<h1>Follow the instructions to reset your password</h1>" + $"<p>To reset your password <a href='{url}'>Click here</a></p>";

        await smtpService.SendEmailAsync(email, "Reset password", emailBody);

        return Result.Success;
    }

}
