using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NDB.Audit.EF.Abstractions;

namespace NDB.Kit.Ef;

/// <summary>
/// EF Core extensions to enforce safer defaults and integrate audit.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Default query entry point with AsNoTracking applied.
    /// Use this for READ-only operations.
    /// </summary>
    public static IQueryable<TEntity> Query<TEntity>(
        this DbContext context)
        where TEntity : class
    {
        return context.Set<TEntity>().AsNoTracking();
    }

    /// <summary>
    /// Save changes and automatically write audit log if IAuditService is registered.
    /// </summary>
    public static async Task<int> SaveWithAuditAsync(
        this DbContext context,
        CancellationToken cancellationToken = default)
    {
        var audit = context.GetService<IAuditService>();
        var result = await context.SaveChangesAsync(cancellationToken);

        if (audit != null)
        {
            await audit.WriteAsync(context, cancellationToken);
        }

        return result;
    }
}
