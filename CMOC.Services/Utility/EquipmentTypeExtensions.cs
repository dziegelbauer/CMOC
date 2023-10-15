using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class EquipmentTypeExtensions
{
    public static EquipmentTypeDto ToDto(this EquipmentType equipmentType)
    {
        var dto = new EquipmentTypeDto
        {
            Id = equipmentType.Id,
            Name = equipmentType.Name
        };

        return dto;
    }

    public static EquipmentType ToEntity(this EquipmentTypeDto dto)
    {
        var equipmentType = new EquipmentType
        {
            Id = dto.Id,
            Name = dto.Name
        };

        return equipmentType;
    }
}