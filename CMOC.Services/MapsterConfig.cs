using System.Net.Http.Headers;
using System.Reflection;
using CMOC.Domain;
using CMOC.Services.Dto;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace CMOC.Services;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration()
    {
        TypeAdapterConfig<ComponentDto, Component>
            .NewConfig()
            .Ignore(dest => dest.Issue)
            .Ignore(dest => dest.Type)
            .Ignore(dest => dest.ComponentOf);
        
        TypeAdapterConfig<EquipmentDto, Equipment>
            .NewConfig()
            .Ignore(dest => dest.Components)
            .Ignore(dest => dest.Relationships)
            .Ignore(dest => dest.Issue)
            .Ignore(dest => dest.Location)
            .Ignore(dest => dest.Type);

        TypeAdapterConfig<ServiceDto, Service>
            .NewConfig()
            .Ignore(dest => dest.Supports)
            .Ignore(dest => dest.SupportedBy);

        TypeAdapterConfig<CapabilityDto, Capability>
            .NewConfig()
            .Ignore(dest => dest.SupportedBy);

        TypeAdapterConfig<Component, ComponentDto>
            .NewConfig()
            .Map(dest => dest.Type, src => src.Type.Name)
            .Map(dest => dest.Issue, src => src.Issue.Adapt<IssueDto>(),
                src => src.IssueId != null)
            .Map(dest => dest.Status, src => src.Operational 
                ? ObjectStatus.FullyCapable 
                : ObjectStatus.NonCapable);

        TypeAdapterConfig<Equipment, EquipmentDto>
            .NewConfig()
            .Map(dest => dest.Components, src => src.Components
                .SelectMany(cr => cr.Components)
                .Select(c => c.Adapt<ComponentDto>())
                .ToList(), src => src.Components.Any())
            .Map(dest => dest.SupportedServices, src => src.Relationships
                .Select(ssr => ssr.Service)
                .Select(s => s.Id)
                .ToList())
            .Map(dest => dest.Location, src => src.Location.Name)
            .Map(dest => dest.IssueId, src => src.IssueId)
            .Map(dest => dest.Issue, src => src.Issue!.Adapt<IssueDto>(), 
                src => src.IssueId != null)
            .Map(dest => dest.Status, src => src.ParseStatusGraph());

        TypeAdapterConfig<Service, ServiceDto>
            .NewConfig()
            .Map(dest => dest.Dependencies, src => src.SupportedBy
                .Select(ssr => ssr.Equipment)
                .Select(e => e.Adapt<EquipmentDto>())
                .ToList())
            .Map(dest => dest.Dependents, src => src.Supports
                .Select(csr => csr.Capability)
                .Select(c => c.Id)
                .ToList())
            .Map(dest => dest.Status, src => src.ParseStatusGraph());

        TypeAdapterConfig<Capability, CapabilityDto>
            .NewConfig()
            .Map(dest => dest.Dependencies, src => src.SupportedBy
                .Select(csr => csr.Service)
                .Select(s => s.Adapt<ServiceDto>())
                .ToList())
            .Map(dest => dest.Status, src => src.ParseStatusGraph());

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
    
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        RegisterMapsterConfiguration();
    }
}