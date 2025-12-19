using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NDB.Abstraction.Requests;
using NDB.Abstraction.Results;

namespace NDB.Kit.Ef;

public static class PagingExtensions
{
    public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
        this IQueryable<TEntity> query,
        PagingRequest paging,
        IMapper mapper,
        CancellationToken ct = default)
        where TEntity : class
    {
        var totalItems = await query.CountAsync(ct);

        var items = await query
            .Skip(paging.Skip)
            .Take(paging.Take)
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return PagedResult<TDto>.Ok(
            items,
            paging.Page,
            paging.PageSize,
            totalItems);
    }
    public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
        this IQueryable<TEntity> query,
        PagingRequest paging,
        Func<TEntity, TDto> selector,
        CancellationToken ct = default)
        where TEntity : class
    {
        var totalItems = await query.CountAsync(ct);

        var entities = await query
            .Skip(paging.Skip)
            .Take(paging.Take)
            .ToListAsync(ct);

        var items = entities.Select(selector).ToList();

        return PagedResult<TDto>.Ok(
            items,
            paging.Page,
            paging.PageSize,
            totalItems);
    }
    public static async Task<ListResult<TDto>> ToListResultAsync<TEntity, TDto>(
    this IQueryable<TEntity> query,
    IMapper mapper,
    CancellationToken ct = default)
    where TEntity : class
    {
        var items = await query
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ListResult<TDto>.Ok(items);
    }

}
