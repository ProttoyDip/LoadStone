namespace Lodestone.Application.Interfaces;

public interface INudgeService
{
    Task GenerateNudgesForAtRiskStudentsAsync(CancellationToken cancellationToken = default);
    Task DispatchPendingNudgesAsync(CancellationToken cancellationToken = default);
}
