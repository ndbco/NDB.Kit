using AutoMapper;
using System.Reflection;

namespace NDB.Kit.Mapping;

public static class AutoMapping
{
    public static void Apply(
        IMapperConfigurationExpression cfg,
        params Assembly[] assemblies)
    {
        var mapMethod = typeof(IMapperConfigurationExpression)
            .GetMethods()
            .First(m =>
                m.Name == nameof(IMapperConfigurationExpression.CreateMap) &&
                m.GetGenericArguments().Length == 2);

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .ToList();

                foreach (var iface in interfaces)
                {
                    var genericDef = iface.GetGenericTypeDefinition();
                    var args = iface.GetGenericArguments();

                    var mappingMethod = type.GetMethod("Mapping");
                    if (mappingMethod == null)
                        continue;

                    var instance = Activator.CreateInstance(type);
                    if (instance == null)
                        continue;

                    if (genericDef == typeof(IMapFrom<,>))
                    {
                        var genericMap = mapMethod.MakeGenericMethod(args[1], args[0]);
                        var map = genericMap.Invoke(cfg, null);
                        mappingMethod.Invoke(instance, new[] { map });
                    }
                    else if (genericDef == typeof(IMapTo<,>))
                    {
                        var genericMap = mapMethod.MakeGenericMethod(args[1], args[0]);
                        var map = genericMap.Invoke(cfg, null);
                        mappingMethod.Invoke(instance, new[] { map });
                    }
                    else if (genericDef == typeof(IMapObject<,>))
                    {
                        var genericMap = mapMethod.MakeGenericMethod(args[0], args[1]);
                        var map = genericMap.Invoke(cfg, null);
                        mappingMethod.Invoke(instance, new[] { map });
                    }
                }
            }
        }
    }
}