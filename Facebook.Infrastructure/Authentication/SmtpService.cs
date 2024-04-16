using Facebook.Application.Common.Interfaces.Authentication;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Facebook.Infrastructure.Authentication;

public class SmtpService : ISmtpService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpService> _logger; // Додавання логгера

    public SmtpService(IConfiguration configuration, ILogger<SmtpService> logger)
    {
        _configuration = configuration;
        _logger = logger; // Ініціалізація логгера
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        string fromEmail = _configuration["EmailSettings:User"]!;
        string SMTP = _configuration["EmailSettings:SMTP"]!;
        int port = int.Parse(_configuration["EmailSettings:port"]!);
        string password = _configuration["EmailSettings:Password"]!;

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
                _logger.LogInformation("Email sent successfully to {toEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {toEmail}", toEmail);
                throw; // Перекиньте виняток далі, якщо потрібно обробити його в іншому місці
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
    }
}
