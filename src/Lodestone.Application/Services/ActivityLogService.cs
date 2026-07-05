using Lodestone.Application.Interfaces;

namespace Lodestone.Application.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly IUnitOfWork _unitOfWork;

    public ActivityLogService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task RecordLoginAsync(int studentProfileId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task IngestBatchAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
