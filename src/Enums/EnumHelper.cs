namespace NDB.Kit.Enums;

/// <summary>
/// Enum parsing helpers.
/// </summary>
public static class EnumHelper
{
    public static bool TryParse<TEnum>(
        string? value,
        out TEnum result,
        bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        return Enum.TryParse(value.Trim(), ignoreCase, out result);
    }

    public static TEnum ParseOrDefault<TEnum>(
        string? value,
        TEnum defaultValue,
        bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        return TryParse(value, out TEnum result, ignoreCase)
            ? result
            : defaultValue;
    }

    public static TEnum ParseOrThrow<TEnum>(
        string value,
        bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (!TryParse(value, out TEnum result, ignoreCase))
            throw new ArgumentException(
                $"Value '{value}' is not valid for enum {typeof(TEnum).Name}");

        return result;
    }
}
