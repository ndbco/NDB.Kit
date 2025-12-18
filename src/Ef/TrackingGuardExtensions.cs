using Microsoft.EntityFrameworkCore;

namespace NDB.Kit.Ef;

/// <summary>
/// Guard helpers to control EF Core tracking behavior.
/// </summary>
public static class TrackingGuardExtensions
{
    /// <summary>
    /// Ensure entity is detached from DbContext.
    /// Useful to avoid accidental tracking.
    /// </summary>
    public static void EnsureDetached<TEntity>(
        this DbContext context,
        TEntity entity)
        where TEntity : class
    {
        var entry = context.Entry(entity);
        if (entry.State != EntityState.Detached)
            entry.State = EntityState.Detached;
    }

    /// <summary>
    /// Ensure entity is tracked.
    /// </summary>
    public static void EnsureTracked<TEntity>(
        this DbContext context,
        TEntity entity)
        where TEntity : class
    {
        var entry = context.Entry(entity);
        if (entry.State == EntityState.Detached)
            context.Attach(entity);
    }
}
