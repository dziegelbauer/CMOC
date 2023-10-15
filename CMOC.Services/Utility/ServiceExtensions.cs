using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class ServiceExtensions
{
    public static ServiceDto ToDto(this Service service)
    {
        var dto = new ServiceDto
        {
            Id = service.Id,
            Dependencies = service.SupportedBy
                .Select(ssr => ssr.Equipment.ToDto())
                .ToList(),
            Dependents = service.Supports
                .Select(csr => csr.Capability.Id)
                .ToList(),
            Name = service.Name,
            Status = service.ParseStatusGraph()
        };

        return dto;
    }

    public static Service ToEntity(this ServiceDto dto)
    {
        var service = new Service
        {
            Id = dto.Id,
            Name = dto.Name
        };
        
        return service;
    }
}