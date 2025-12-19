using System.Linq.Expressions;

namespace NDB.Kit.Ef;

public static class QuerySearchExtensions
{
    public static IQueryable<T> ApplySearch<T>(
        this IQueryable<T> query,
        string? keyword,
        IReadOnlySet<string> searchableFields)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? combined = null;

        foreach (var field in searchableFields)
        {
            var property = typeof(T).GetProperty(field);
            if (property == null || property.PropertyType != typeof(string))
                continue;


            var member = Expression.Property(parameter, property);
            var toLower = Expression.Call(member, nameof(string.ToLower), null);
            var value = Expression.Constant(keyword.ToLower());

            var contains = Expression.Call(
                            toLower,
                            nameof(string.Contains),
                            Type.EmptyTypes,
                            value);


            combined = combined == null
                ? contains
                : Expression.OrElse(combined, contains);
        }

        if (combined == null)
            return query;

        var predicate =
            Expression.Lambda<Func<T, bool>>(combined, parameter);

        return query.Where(predicate);
    }
}
