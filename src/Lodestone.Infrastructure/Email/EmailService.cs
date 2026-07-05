using Lodestone.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Lodestone.Infrastructure.Email;

/// <summary>SMTP implementation of the Application <see cref="IEmailService"/> contract.</summary>
public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings) => _settings = settings.Value;

    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
