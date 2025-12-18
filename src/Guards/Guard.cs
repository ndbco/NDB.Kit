namespace NDB.Kit.Guards;

/// <summary>
/// Guard clauses for validating method arguments.
/// </summary>
public static class Guard
{
    public static void AgainstNull(object? value, string name)
    {
        if (value is null)
            throw new ArgumentNullException(name);
    }

    public static void AgainstEmpty(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{name} cannot be empty");
    }

    public static void AgainstDefault<T>(T value, string name)
        where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new ArgumentException($"{name} cannot be default");
    }

    public static void AgainstNegative(int value, string name)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(name);
    }
}
