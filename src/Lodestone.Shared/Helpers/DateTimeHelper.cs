namespace Lodestone.Shared.Helpers;

public static class DateTimeHelper
{
    public static int DaysSince(DateTime fromUtc, DateTime nowUtc)
        => (int)Math.Floor((nowUtc - fromUtc).TotalDays);

    public static DateTime StartOfWeekUtc(DateTime utc)
        => utc.Date.AddDays(-(int)utc.DayOfWeek);
}
