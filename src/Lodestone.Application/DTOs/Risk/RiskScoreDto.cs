using Lodestone.Domain.Enums;

namespace Lodestone.Application.DTOs.Risk;

public record RiskScoreDto(
    int StudentProfileId,
    string StudentName,
    double Probability,
    RiskLevel Level,
    DateTime ScoredAtUtc);
