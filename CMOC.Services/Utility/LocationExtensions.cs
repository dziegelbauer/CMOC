using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class LocationExtensions
{
    public static LocationDto ToDto(this Location location)
    {
        var dto = new LocationDto
        {
            Id = location.Id,
            Name = location.Name
        };

        return dto;
    }

    public static Location ToEntity(this LocationDto dto)
    {
        var location = new Location
        {
            Id = dto.Id,
            Name = dto.Name
        };

        return location;
    }
}