using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Repository;

public interface IEquipmentRepository : IAssetRepository<Equipment, EquipmentDto, EquipmentType, EquipmentTypeDto>
{
}