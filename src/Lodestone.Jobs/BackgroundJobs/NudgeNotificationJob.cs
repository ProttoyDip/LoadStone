using Lodestone.Application.Interfaces;

namespace Lodestone.Jobs.BackgroundJobs;

public class NudgeNotificationJob
{
    private readonly INudgeService _nudgeService;

    public NudgeNotificationJob(INudgeService nudgeService) => _nudgeService = nudgeService;

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
        => _nudgeService.DispatchPendingNudgesAsync(cancellationToken);
}
