namespace NDB.Kit.Identifiers;
public static class IdGenerator
{
    public static string NewId()
        => Guid.NewGuid().ToString("N").ToUpperInvariant();
}
