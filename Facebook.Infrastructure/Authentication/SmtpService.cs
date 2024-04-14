using Facebook.Application.Common.Interfaces.Authentication;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Facebook.Infrastructure.Authentication;

public class SmtpService : ISmtpService
{
    private readonly IConfiguration _configuration;

    public SmtpService(IConfiguration configuration)
    {
        _configuration = configuration;
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
            smtp.Connect(SMTP, port, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(fromEmail, password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}