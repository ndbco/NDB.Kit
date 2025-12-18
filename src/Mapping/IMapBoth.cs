namespace NDB.Kit.Mapping;

/// <summary>
/// Marker interface for two-way mapping.
/// </summary>
public interface IMapBoth<TEntity> :
    IMapFrom<TEntity>,
    IMapTo<TEntity>
{
}
