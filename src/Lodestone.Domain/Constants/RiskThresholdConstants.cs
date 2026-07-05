namespace Lodestone.Domain.Constants;

/// <summary>Score cut-offs that map a raw model probability to a <see cref="Enums.RiskLevel"/>.</summary>
public static class RiskThresholdConstants
{
    public const double LowUpperBound = 0.25;
    public const double ModerateUpperBound = 0.50;
    public const double HighUpperBound = 0.75;
    // Anything above HighUpperBound is Critical.
}
