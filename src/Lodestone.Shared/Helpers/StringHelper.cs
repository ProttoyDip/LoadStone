namespace Lodestone.Shared.Helpers;

public static class StringHelper
{
    public static string Truncate(string value, int maxLength)
        => string.IsNullOrEmpty(value) || value.Length <= maxLength
            ? value
            : value[..maxLength] + "…";

    public static bool HasValue(string? value) => !string.IsNullOrWhiteSpace(value);
}
