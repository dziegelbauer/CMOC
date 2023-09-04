using CMOC.Services.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CMOC.Services;

public static class ServiceConfig
{
    public static void RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ICapabilityRepository, CapabilityRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IComponentRepository, ComponentRepository>();
        services.AddScoped<IIssueRepository, IssueRepository>();
        services.AddScoped<IObjectManager, ObjectManager>();
    }
}