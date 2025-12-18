namespace NDB.Kit.Collections;

/// <summary>
/// Collection helper extensions.
/// </summary>
public static class EnumerableExtensions
{
    public static bool None<T>(this IEnumerable<T> source)
        => !source.Any();

    public static bool None<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
        => !source.Any(predicate);

    public static IEnumerable<(int Index, T Item)> WithIndex<T>(
        this IEnumerable<T> source,
        int start = 1)
    {
        foreach (var item in source)
            yield return (start++, item);
    }
}
