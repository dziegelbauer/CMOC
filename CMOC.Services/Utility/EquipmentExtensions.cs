using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class EquipmentExtensions
{
    public static EquipmentDto ToDto(this Equipment equipment)
    {
        var dto = new EquipmentDto
        {
            Id = equipment.Id,
            SerialNumber = equipment.SerialNumber,
            Components = equipment.Components
                .SelectMany(cr => cr.Components)
                .Select(c => c.ToDto())
                .ToList(),
            TypeId = equipment.TypeId,
            TypeName = equipment.Type.Name,
            LocationId = equipment.LocationId,
            Location = equipment.Location.Name,
            SupportedServices = equipment.Relationships
                .Select(ssr => ssr.Service)
                .Select(s => s.Id)
                .ToList(),
            Notes = equipment.Notes,
            OperationalOverride = equipment.OperationalOverride,
            IssueId = equipment.IssueId,
            Issue = equipment.Issue?.ToDto(),
            Status = equipment.ParseStatusGraph()
        };

        return dto;
    }

    public static Equipment ToEntity(this EquipmentDto dto)
    {
        var equipment = new Equipment
        {
            Id = dto.Id,
            SerialNumber = dto.SerialNumber,
            TypeId = dto.TypeId,
            LocationId = dto.LocationId,
            IssueId = dto.IssueId,
            Notes = dto.Notes,
            OperationalOverride = dto.OperationalOverride
        };

        return equipment;
    }
}