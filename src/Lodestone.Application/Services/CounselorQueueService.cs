using Lodestone.Application.DTOs.Risk;
using Lodestone.Application.Interfaces;

namespace Lodestone.Application.Services;

public class CounselorQueueService : ICounselorQueueService
{
    private readonly IUnitOfWork _unitOfWork;

    public CounselorQueueService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<IReadOnlyList<RiskQueueItemDto>> GetQueueAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task ResolveAsync(int queueEntryId, string resolvedByUserId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
