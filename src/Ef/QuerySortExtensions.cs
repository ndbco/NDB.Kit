using System.Linq.Expressions;
using NDB.Abstraction.Requests;

namespace NDB.Kit.Ef;

public static class QuerySortExtensions
{
    public static IQueryable<T> ApplySorts<T>(
    this IQueryable<T> query,
    IEnumerable<SortRequest> sorts,
    IReadOnlySet<string> allowedFields)
    {
        bool first = true;

        foreach (var sort in sorts.Where(s =>
            s.IsValid && allowedFields.Contains(s.Field)))
        {
            query = ApplySort(query, sort, first);
            first = false;
        }

        return query;
    }


    private static IQueryable<T> ApplySort<T>(
        IQueryable<T> query,
        SortRequest sort,
        bool first)
    {
        var property = typeof(T).GetProperty(sort.Field);
        if (property == null)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var member = Expression.Property(parameter, property);
        var keySelector =
            Expression.Lambda(member, parameter);

        var methodName = GetMethodName(sort.Direction, first);

        var method = typeof(Queryable).GetMethods()
            .First(m =>
                m.Name == methodName &&
                m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.PropertyType);

        return (IQueryable<T>)method.Invoke(
            null,
            new object[] { query, keySelector })!;
    }

    private static string GetMethodName(
        SortDirection direction,
        bool first)
    {
        if (first)
            return direction == SortDirection.Asc
                ? "OrderBy"
                : "OrderByDescending";

        return direction == SortDirection.Asc
            ? "ThenBy"
            : "ThenByDescending";
    }
}
