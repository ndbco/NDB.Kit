using System.Globalization;

namespace NDB.Kit.Primitives;

/// <summary>
/// Safe parsing helpers for primitive types.
/// </summary>
public static class Parse
{
    public static int? Int(string? value)
        => int.TryParse(value, out var v) ? v : null;

    public static long? Long(string? value)
        => long.TryParse(value, out var v) ? v : null;

    public static decimal? Decimal(string? value)
        => decimal.TryParse(
            value,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out var v)
            ? v
            : null;

    public static bool? Bool(string? value)
        => bool.TryParse(value, out var v) ? v : null;

    public static Guid? Guid(string? value)
        => System.Guid.TryParse(value, out var v) ? v : null;

    public static DateTime? DateTime(string? value)
        => System.DateTime.TryParse(value, out var v) ? v : null;
}
