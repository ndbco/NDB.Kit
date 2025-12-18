using AutoMapper;
using System.Reflection;

namespace NDB.Kit.Mapping;

public sealed class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetCallingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes();

        foreach (var type in types)
        {
            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType) continue;

                var definition = iface.GetGenericTypeDefinition();
                var entityType = iface.GetGenericArguments()[0];

                // Entity -> Model
                if (definition == typeof(IMapFrom<>))
                {
                    var map = CreateMap(entityType, type);
                    InvokeMapping(type, "Mapping", map);
                }

                // Model -> Entity
                if (definition == typeof(IMapTo<>))
                {
                    var map = CreateMap(type, entityType);
                    InvokeMapping(type, "Mapping", map);
                }
            }
        }
    }

    private static void InvokeMapping(
        Type type,
        string methodName,
        object map)
    {
        var instance = Activator.CreateInstance(type);
        var method = type.GetMethod(methodName);
        method?.Invoke(instance, new[] { map });
    }
}
