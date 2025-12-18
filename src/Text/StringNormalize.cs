namespace NDB.Kit.Text;

/// <summary>
/// String normalization helpers.
/// </summary>
public static class StringNormalize
{
    public static string Normalize(
        string? input,
        bool removeWhitespace = false,
        bool upper = false)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var result = input
            .Replace("\r", " ")
            .Replace("\n", " ")
            .Replace("\t", " ")
            .Trim();

        if (removeWhitespace)
            result = result.Replace(" ", "");

        if (upper)
            result = result.ToUpperInvariant();

        return result;
    }
}
