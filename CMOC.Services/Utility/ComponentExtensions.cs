using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class ComponentExtensions
{
    public static ComponentDto ToDto(this Component component)
    {
        var dto = new ComponentDto
        {
            Id = component.Id,
            SerialNumber = component.SerialNumber,
            TypeId = component.TypeId,
            TypeName = component.Type.Name,
            Operational = component.Operational,
            ComponentOfId = component.ComponentOfId,
            EquipmentId = component.ComponentOf.EquipmentId,
            Equipment = $"{component.ComponentOf.Equipment.Type.Name} ({component.ComponentOf.Equipment.SerialNumber})",
            IssueId = component.IssueId,
            Issue = component.Issue?.ToDto(),
            Status = component.Operational 
                ? ObjectStatus.FullyCapable 
                : ObjectStatus.NonCapable
        };

        return dto;
    }

    public static Component ToEntity(this ComponentDto dto)
    {
        var component = new Component()
        {
            Id = dto.Id,
            SerialNumber = dto.SerialNumber,
            TypeId = dto.TypeId,
            Operational = dto.Operational,
            ComponentOfId = dto.ComponentOfId,
            IssueId = dto.IssueId
        };

        return component;
    }
}