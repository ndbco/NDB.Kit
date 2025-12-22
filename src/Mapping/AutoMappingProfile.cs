using AutoMapper;
using System.Reflection;

namespace NDB.Kit.Mapping;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            ApplyMappingsFromAssembly(assembly);
    }
    public void ApplyMappingsFromAssembly(Assembly assembly)
    {
        MethodInfo mapMethod = this.GetType()
            .GetMethods()
            .First(i => i.Name == "CreateMap" &&
                        i.GetGenericArguments().Length == 2);

        var types_response = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapFrom<,>)))
            .ToList();

        var types_request = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapTo<,>)))
            .ToList();


        var types_object = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapObject<,>)))
            .ToList();

        foreach (var type in types_response)
        {
            var mapInterface = type.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(IMapFrom<,>));

            var arguments = mapInterface.GetGenericArguments();

            var instance = Activator.CreateInstance(type)
                ?? throw new InvalidOperationException(
                    $"Cannot create instance of {type.FullName}");

            var methodInfo = type.GetMethod("Mapping")
                ?? throw new InvalidOperationException(
                    $"Mapping method not found on {type.FullName}");

            var genericMapMethod = mapMethod.MakeGenericMethod(arguments[1], arguments[0]);
            var map = genericMapMethod.Invoke(this, null)
                ?? throw new InvalidOperationException("CreateMap returned null");

            methodInfo.Invoke(instance, new[] { map });
        }

        foreach (var type in types_request)
        {
            var mapInterface = type.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(IMapTo<,>));

            var arguments = mapInterface.GetGenericArguments();

            var instance = Activator.CreateInstance(type)
                ?? throw new InvalidOperationException(
                    $"Cannot create instance of {type.FullName}");

            var methodInfo = type.GetMethod("Mapping")
                ?? throw new InvalidOperationException(
                    $"Mapping method not found on {type.FullName}");

            var genericMapMethod = mapMethod.MakeGenericMethod(arguments[1], arguments[0]);
            var map = genericMapMethod.Invoke(this, null)
                ?? throw new InvalidOperationException("CreateMap returned null");

            methodInfo.Invoke(instance, new[] { map });
        }

        foreach (var type in types_object)
        {
            var mapInterface = type.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(IMapObject<,>));

            var arguments = mapInterface.GetGenericArguments();

            var instance = Activator.CreateInstance(type)
                ?? throw new InvalidOperationException(
                    $"Cannot create instance of {type.FullName}");

            var methodInfo = type.GetMethod("Mapping")
                ?? throw new InvalidOperationException(
                    $"Mapping method not found on {type.FullName}");
            var genericMapMethod = mapMethod.MakeGenericMethod(arguments[0], arguments[1]);
            var map = genericMapMethod.Invoke(this, null)
                ?? throw new InvalidOperationException("CreateMap returned null");

            methodInfo.Invoke(instance, new[] { map });
        }
    }
}