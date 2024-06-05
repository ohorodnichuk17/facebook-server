using System.Net;
using System.Text;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.WebUtilities;

namespace Facebook.Application.Services;

public class EmailService(ISmtpService _smtpService)
{
    public async Task<ErrorOr<Success>> SendEmailConfirmationEmailAsync(
        Guid userId, string email, string token, string baseUrl, string userName)
    {
        string url = $"{baseUrl}api/authentication/confirm-email?userId={userId}&" +
            $"validEmailToken={WebUtility.UrlEncode(token)}";

        string confirmationUrl = $" <a href='{url}'>Confirm now</a>";

        string emailBody = string.Empty;

        using (StreamReader reader = new("./EmailTemplates/email-confirmation.html"))
        {
            emailBody = reader.ReadToEnd();
        }

        emailBody = emailBody.Replace("{{ name }}", userName);

        emailBody = emailBody.Replace("{{ code }}", confirmationUrl);

        await _smtpService.SendEmailAsync(email, "Email confirmation.", emailBody);

        return Result.Success;
    }
    
    public async Task<ErrorOr<Success>> SendResetPasswordEmailAsync(
        string email, string token, string baseUrl, string userName, bool isReset = true)
    {
        string endpoint = isReset ? "reset-password" : "forgot-password";
        string url = $"{baseUrl}api/authentication/{endpoint}?email={email}&" +
                     $"validEmailToken={WebUtility.UrlEncode(token)}";

        WebUtility.UrlEncode(token);
        string emailTemplate = isReset ? "./EmailTemplates/reset-password-success.html" : "./EmailTemplates/forgot-password.html";

        string emailBody = string.Empty;

        using (StreamReader reader = new(emailTemplate))
        {
            emailBody = await reader.ReadToEndAsync();
        }

        emailBody = emailBody.Replace("{{ name }}", userName)
            .Replace("{{ url }}", url);

        string emailSubject = isReset ? "Reset password" : "Forgot password";
    
        await _smtpService.SendEmailAsync(email, emailSubject, emailBody);

        return Result.Success;
    }


    public async Task<ErrorOr<Success>> SendChangeEmailEmailAsync(
        string email, string token, string baseUrl, 
        string userName, string userId)
    {
        // string url = $"{baseUrl}api/authentication/confirm-email?userId={userId}&" +
        //              $"validEmailToken={WebUtility.UrlEncode(token)}";
        string url = $"{baseUrl}api/authentication/change-email?userId={userId}&" + 
                     $"email={email}&" + 
                     $"token={WebUtility.UrlEncode(token)}";
        
        string emailBody = string.Empty;

        using (StreamReader reader = new("./EmailTemplates/email-change.html"))
        {
            emailBody = reader.ReadToEnd();
        }

        emailBody = emailBody.Replace("{{ name }}", userName);

        emailBody = emailBody.Replace("{{ url }}", url);

        await _smtpService.SendEmailAsync(email, "Change Email", emailBody);

        return Result.Success;
    }
}
