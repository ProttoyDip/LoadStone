using Lodestone.Application.Interfaces;

namespace Lodestone.Jobs.BackgroundJobs;

/// <summary>Escalates critical-risk students to counselors / crisis workflows.</summary>
public class CrisisResourceEscalationJob
{
    private readonly ICounselorQueueService _counselorQueueService;

    public CrisisResourceEscalationJob(ICounselorQueueService counselorQueueService)
        => _counselorQueueService = counselorQueueService;

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
