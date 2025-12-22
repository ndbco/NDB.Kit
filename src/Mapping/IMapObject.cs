using AutoMapper;
using NDB.Abstraction.Markers;

namespace NDB.Kit.Mapping;

/// <summary>
/// Implement this interface to define mapping TO an entity from this model.
/// </summary>
/// <typeparam name="TEntity">Destination entity</typeparam>
public interface IMapObject<TSource, TDestination>
{
    void Mapping(IMappingExpression<TSource, TDestination> map);
}
