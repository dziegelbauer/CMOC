using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Repository;

public interface IComponentRepository : IAssetRepository<Component, ComponentDto, ComponentType, ComponentTypeDto>
{
    
}