using AutoMapper;

namespace NDB.Kit.Mapping;

/// <summary>
/// Implement this interface to define mapping TO an entity from this model.
/// </summary>
/// <typeparam name="TEntity">Destination entity</typeparam>
public interface IMapTo<TEntity>
{
    void Mapping(IMappingExpression<object, TEntity> map);
}
