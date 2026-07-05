using Lodestone.Application.Interfaces;

namespace Lodestone.Jobs.BackgroundJobs;

public class BookingReminderJob
{
    private readonly IEmailService _emailService;

    public BookingReminderJob(IEmailService emailService) => _emailService = emailService;

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
