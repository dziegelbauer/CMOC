using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class ComponentTypeExtensions
{
    public static ComponentTypeDto ToDto(this ComponentType componentType)
    {
        var dto = new ComponentTypeDto
        {
            Id = componentType.Id,
            Name = componentType.Name
        };

        return dto;
    }

    public static ComponentType ToEntity(this ComponentTypeDto dto)
    {
        var componentType = new ComponentType
        {
            Id = dto.Id,
            Name = dto.Name
        };

        return componentType;
    }
}