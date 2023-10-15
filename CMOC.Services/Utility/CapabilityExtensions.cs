using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class CapabilityExtensions
{
    public static CapabilityDto ToDto(this Capability capability)
    {
        var dto = new CapabilityDto
        {
            Id = capability.Id,
            Dependencies = capability.SupportedBy
                .Select(csr => csr.Service.ToDto())
                .ToList(),
            Name = capability.Name,
            Status = capability.ParseStatusGraph()
        };
        
        return dto;
    }

    public static Capability ToEntity(this CapabilityDto dto)
    {
        var capability = new Capability
        {
            Id = dto.Id,
            Name = dto.Name
        };

        return capability;
    }
}