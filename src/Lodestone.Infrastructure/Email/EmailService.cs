using System.Net;
using System.Net.Mail;
using Lodestone.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Lodestone.Infrastructure.Email;

/// <summary>SMTP implementation of the Application <see cref="IEmailService"/> contract.</summary>
public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings) => _settings = settings.Value;

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = _settings.SmtpPort != 25,
            Credentials = string.IsNullOrWhiteSpace(_settings.UserName)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(_settings.UserName, _settings.Password)
        };

        await client.SendMailAsync(message, cancellationToken);
    }
}
