using System.Linq.Expressions;
using NDB.Abstraction.Requests;

namespace NDB.Kit.Ef;

public static class QueryFilterExtensions
{
    public static IQueryable<T> ApplyFilters<T>(
     this IQueryable<T> query,
     IEnumerable<FilterRequest> filters,
     IReadOnlySet<string> allowedFields)
    {
        foreach (var filter in filters.Where(f =>
            f.IsValid && allowedFields.Contains(f.Field)))
        {
            query = query.ApplyFilter(filter);
        }

        return query;
    }

    private static IQueryable<T> ApplyFilter<T>(
        this IQueryable<T> query,
        FilterRequest filter)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var property = typeof(T).GetProperty(filter.Field);
        if (property == null)
            return query; // silently ignore unknown field

        var member = Expression.Property(parameter, property);

        object? value;
        try
        {
            value = Convert.ChangeType(filter.Value, property.PropertyType);
        }
        catch
        {
            return query;
        }

        var constant = Expression.Constant(value);

        Expression body = filter.Operator switch
        {
            FilterOperator.Equals =>
                Expression.Equal(member, constant),

            FilterOperator.Contains =>
                BuildStringCall(member, "Contains", constant),

            FilterOperator.StartsWith =>
                BuildStringCall(member, "StartsWith", constant),

            FilterOperator.EndsWith =>
                BuildStringCall(member, "EndsWith", constant),

            FilterOperator.GreaterThan =>
                Expression.GreaterThan(member, constant),

            FilterOperator.LessThan =>
                Expression.LessThan(member, constant),

            _ => null!
        };

        if (body == null)
            return query;

        var predicate =
            Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(predicate);
    }

    private static Expression BuildStringCall(
        MemberExpression member,
        string method,
        ConstantExpression constant)
    {
        if (member.Type != typeof(string))
            return null!;

        return Expression.Call(member, method, Type.EmptyTypes, constant);
    }
}
