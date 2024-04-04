namespace Facebook.Application.Common.Interfaces.Authentication;

public interface ISmtpService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}