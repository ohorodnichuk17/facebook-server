using Facebook.Application.Common.Interfaces.Authentication;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Facebook.Infrastructure.Authentication;

public class SmtpService(IConfiguration configuration, ILogger<SmtpService> logger) : ISmtpService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        string fromEmail = configuration["EmailSettings:User"]!;
        string SMTP = configuration["EmailSettings:SMTP"]!;
        int port = int.Parse(configuration["EmailSettings:port"]!);
        string password = configuration["EmailSettings:Password"]!;

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(fromEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        email.Body = bodyBuilder.ToMessageBody();

        using (var smtp = new SmtpClient())
        {
            try
            {
                smtp.Connect(SMTP, port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(fromEmail, password);
                await smtp.SendAsync(email);
                logger.LogInformation("Email sent successfully to {toEmail}", toEmail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending email to {toEmail}", toEmail);
                throw; 
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
    }
}
