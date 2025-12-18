using AutoMapper;

namespace NDB.Kit.Mapping;

/// <summary>
/// Implement this interface to define mapping FROM an entity to this model.
/// </summary>
/// <typeparam name="TEntity">Source entity</typeparam>
public interface IMapFrom<TEntity>
{
    void Mapping(IMappingExpression<TEntity, object> map);
}
