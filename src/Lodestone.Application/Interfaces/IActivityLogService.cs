namespace Lodestone.Application.Interfaces;

public interface IActivityLogService
{
    Task RecordLoginAsync(int studentProfileId, CancellationToken cancellationToken = default);
    Task IngestBatchAsync(CancellationToken cancellationToken = default);
}
