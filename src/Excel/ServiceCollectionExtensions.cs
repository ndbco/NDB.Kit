using Microsoft.Extensions.DependencyInjection;
namespace NDB.Kit.Excel;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNdbExcel(this IServiceCollection services)
    {
        services.AddSingleton<ExcelHelper>();
        services.AddSingleton<ExcelExporter>();
        return services;
    }
}
