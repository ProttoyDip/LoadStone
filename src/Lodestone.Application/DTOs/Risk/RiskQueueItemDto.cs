using Lodestone.Domain.Enums;

namespace Lodestone.Application.DTOs.Risk;

public record RiskQueueItemDto(
    int QueueEntryId,
    int StudentProfileId,
    string StudentName,
    RiskLevel Level,
    bool IsResolved,
    DateTime CreatedAtUtc);
