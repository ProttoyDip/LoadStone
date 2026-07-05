using Lodestone.Application.DTOs.Risk;

namespace Lodestone.Application.Interfaces;

public interface ICounselorQueueService
{
    Task<IReadOnlyList<RiskQueueItemDto>> GetQueueAsync(CancellationToken cancellationToken = default);
    Task ResolveAsync(int queueEntryId, string resolvedByUserId, CancellationToken cancellationToken = default);
}
