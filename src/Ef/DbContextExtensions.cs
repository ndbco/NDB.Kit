using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NDB.Audit.EF.Abstractions;
using NDB.Audit.EF.Models;

namespace NDB.Kit.Ef;

public sealed record AuditSaveResult<TAudit>(
    bool Success,
    string Message,
    Exception? Exception,
    IReadOnlyList<TAudit> AuditEntries)
{
    public static AuditSaveResult<TAudit> Ok(
        IReadOnlyList<TAudit> audits)
        => new(true, "OK", null, audits);

    public static AuditSaveResult<TAudit> Fail(Exception ex)
        => new(false, ex.Message, ex, Array.Empty<TAudit>());
}


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
    public static async Task<AuditSaveResult<AuditEntry>> SaveWithAuditResultAsync(
         this DbContext context,
         CancellationToken ct = default)
    {
        try
        {
            var auditService = context.GetService<IAuditService>();

            var audits = auditService != null
                ? await auditService.WriteWithResultAsync(context, ct)
                : Array.Empty<AuditEntry>();

            await context.SaveChangesAsync(ct);

            return AuditSaveResult<AuditEntry>.Ok(audits);
        }
        catch (Exception ex)
        {
            return AuditSaveResult<AuditEntry>.Fail(ex);
        }
    }
}
