using System.Globalization;

namespace ChildrenPath.Platform.Drivers.Interfaces.Transform;

internal static class DriverResourceDateParser
{
    public static DateTime? ParseOptionalDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var date))
            return date;

        throw new InvalidOperationException("License expiration date must be a valid ISO-8601 date.");
    }
}