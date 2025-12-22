using AutoMapper;
using NDB.Abstraction.Markers;

namespace NDB.Kit.Mapping;

/// <summary>
/// Implement this interface to define mapping FROM an entity to this model.
/// </summary>
/// <typeparam name="TEntity">Source entity</typeparam>
public interface IMapFrom<TModel, TEntity> where TEntity : IEntity
{
    public void Mapping(IMappingExpression<TEntity, TModel> map);
}