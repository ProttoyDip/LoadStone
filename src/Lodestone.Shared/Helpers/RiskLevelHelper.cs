namespace Lodestone.Shared.Helpers;

/// <summary>Maps a model probability to a display bucket. Kept string-based to avoid a Domain dependency.</summary>
public static class RiskLevelHelper
{
    public static string DescribeProbability(double probability) => probability switch
    {
        < 0.25 => "Low",
        < 0.50 => "Moderate",
        < 0.75 => "High",
        _ => "Critical"
    };
}
