using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Repository;

namespace CMOC.Services;

public class ObjectManager : IObjectManager
{
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IComponentRepository _componentRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly AppDbContext _db;

    public ObjectManager(
        AppDbContext db, 
        ICapabilityRepository capabilityRepository, 
        IServiceRepository serviceRepository, 
        IEquipmentRepository equipmentRepository, 
        ILocationRepository locationRepository, 
        IComponentRepository componentRepository, 
        IIssueRepository issueRepository)
    {
        _db = db;
        _capabilityRepository = capabilityRepository;
        _serviceRepository = serviceRepository;
        _equipmentRepository = equipmentRepository;
        _locationRepository = locationRepository;
        _componentRepository = componentRepository;
        _issueRepository = issueRepository;
    }

    #region Capability Methods
    public async Task<ServiceResponse<CapabilityDto>> GetCapabilityAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        return await _capabilityRepository.GetAsync(filter);
    }

    public async Task<ServiceResponse<List<CapabilityDto>>> GetCapabilitiesAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        return await _capabilityRepository.GetManyAsync(filter);
    }

    public async Task<ServiceResponse<CapabilityDto>> AddCapabilityAsync(CapabilityDto dto)
    {
        return await _capabilityRepository.AddAsync(dto);
    }

    public async Task<ServiceResponse<CapabilityDto>> UpdateCapabilityAsync(CapabilityDto dto)
    {
        return await _capabilityRepository.UpdateAsync(dto);
    }

    public async Task<ServiceResponse<CapabilityDto>> RemoveCapabilityAsync(int id)
    {
        return await _capabilityRepository.RemoveAsync(id);
    }

    #endregion

    #region Service Methods
    public async Task<ServiceResponse<ServiceDto>> GetServiceAsync(Expression<Func<Service, bool>>? filter = null)
    {
        return await _serviceRepository.GetAsync(filter);
    }

    public async Task<ServiceResponse<List<ServiceDto>>> GetServicesAsync(Expression<Func<Service, bool>>? filter = null)
    {
        return await _serviceRepository.GetManyAsync(filter);
    }

    public async Task<ServiceResponse<ServiceDto>> AddServiceAsync(ServiceDto dto)
    {
        return await _serviceRepository.AddAsync(dto);
    }

    public async Task<ServiceResponse<ServiceDto>> UpdateServiceAsync(ServiceDto dto)
    {
        return await _serviceRepository.UpdateAsync(dto);
    }

    public async Task<ServiceResponse<ServiceDto>> RemoveServiceAsync(int id)
    {
        return await _serviceRepository.RemoveAsync(id);
    }

    #endregion

    #region Location Methods
    public async Task<ServiceResponse<LocationDto>> GetLocationAsync(Expression<Func<Location, bool>>? filter = null)
    {
        return await _locationRepository.GetAsync(filter);
    }

    public async Task<ServiceResponse<List<LocationDto>>> GetLocationsAsync(Expression<Func<Location, bool>>? filter = null)
    {
        return await _locationRepository.GetManyAsync(filter);
    }

    public async Task<ServiceResponse<LocationDto>> AddLocationAsync(LocationDto dto)
    {
        return await _locationRepository.AddAsync(dto);
    }

    public async Task<ServiceResponse<LocationDto>> UpdateLocationAsync(LocationDto dto)
    {
        return await _locationRepository.UpdateAsync(dto);
    }

    public async Task<ServiceResponse<LocationDto>> RemoveLocationAsync(int id)
    {
        return await _locationRepository.RemoveAsync(id);
    }

    #endregion

    #region Equipment Methods
    public async Task<ServiceResponse<EquipmentDto>> GetEquipmentItemAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        return await _equipmentRepository.GetAsync(filter);
    }

    public async Task<ServiceResponse<List<EquipmentDto>>> GetEquipmentItemsAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        return await _equipmentRepository.GetManyAsync(filter);
    }

    public async Task<ServiceResponse<EquipmentDto>> AddEquipmentItemAsync(EquipmentDto dto)
    {
        return await _equipmentRepository.AddAsync(dto);
    }

    public async Task<ServiceResponse<EquipmentDto>> UpdateEquipmentItemAsync(EquipmentDto dto)
    {
        return await _equipmentRepository.UpdateAsync(dto);
    }

    public async Task<ServiceResponse<EquipmentDto>> RemoveEquipmentItemAsync(int id)
    {
        return await _equipmentRepository.RemoveAsync(id);
    }

    #endregion

    #region EquipmentType Methods
    public async Task<ServiceResponse<EquipmentTypeDto>> GetEquipmentTypeAsync(Expression<Func<EquipmentType, bool>>? filter = null)
    {
        return await _equipmentRepository.GetTypeAsync(filter);
    }

    public async Task<ServiceResponse<List<EquipmentTypeDto>>> GetEquipmentTypesAsync(
        Expression<Func<EquipmentType, bool>>? filter = null)
    {
        return await _equipmentRepository.GetTypesAsync(filter);
    }

    public async Task<ServiceResponse<EquipmentTypeDto>> AddEquipmentTypeAsync(EquipmentTypeDto dto)
    {
        return await _equipmentRepository.AddTypeAsync(dto);
    }

    public async Task<ServiceResponse<EquipmentTypeDto>> UpdateEquipmentTypeAsync(EquipmentTypeDto dto)
    {
        return await _equipmentRepository.UpdateTypeAsync(dto);
    }

    public async Task<ServiceResponse<EquipmentTypeDto>> RemoveEquipmentTypeAsync(int id)
    {
        return await _equipmentRepository.RemoveTypeAsync(id);
    }

    public async Task<ServiceResponse<EquipmentDto>> AssignIssueToEquipment(int equipmentId, int issueId)
    {
        var equipmentResponse = await _equipmentRepository.GetAsync(e => e.Id == equipmentId);

        if (equipmentResponse.Result != ServiceResult.Success)
        {
            return equipmentResponse;
        }
        
        var issueResponse = await _issueRepository.GetAsync(i => i.Id == issueId);

        if (issueResponse.Result != ServiceResult.Success)
        {
            return new ServiceResponse<EquipmentDto>
            {
                Result = issueResponse.Result,
                Payload = null,
                Message = issueResponse.Message
            };
        }

        equipmentResponse.Payload!.IssueId = issueId;

        var updatedEquipment = await _equipmentRepository.UpdateAsync(equipmentResponse.Payload);

        if (updatedEquipment.Result != ServiceResult.Success)
        {
            return updatedEquipment;
        }

        return await _equipmentRepository.GetAsync(e => e.Id == equipmentId);
    }
    
    #endregion

    #region Component Methods

    public async Task<ServiceResponse<ComponentDto>> GetComponentAsync(Expression<Func<Component, bool>>? filter = null)
    {
        return await _componentRepository.GetAsync(filter);
    }

    public async Task<ServiceResponse<List<ComponentDto>>> GetComponentsAsync(Expression<Func<Component, bool>>? filter = null)
    {
        return await _componentRepository.GetManyAsync(filter);
    }

    public async Task<ServiceResponse<ComponentDto>> AddComponentAsync(ComponentDto dto)
    {
        return await _componentRepository.AddAsync(dto);
    }

    public async Task<ServiceResponse<ComponentDto>> UpdateComponentAsync(ComponentDto dto)
    {
        return await _componentRepository.UpdateAsync(dto);
    }

    public async Task<ServiceResponse<ComponentDto>> RemoveComponentAsync(int id)
    {
        return await _componentRepository.RemoveAsync(id);
    }

    #endregion

    #region ComponentType Methods
    public async Task<ServiceResponse<ComponentTypeDto>> GetComponentTypeAsync(Expression<Func<ComponentType, bool>>? filter = null)
    {
        return await _componentRepository.GetTypeAsync(filter);
    }
    public async Task<ServiceResponse<List<ComponentTypeDto>>> GetComponentTypesAsync(Expression<Func<ComponentType, bool>>? filter = null)
    {
        return await _componentRepository.GetTypesAsync(filter);
    }
    public async Task<ServiceResponse<ComponentTypeDto>> AddComponentTypeAsync(ComponentTypeDto dto)
    {
        return await _componentRepository.AddTypeAsync(dto);
    }

    public async Task<ServiceResponse<ComponentTypeDto>> UpdateComponentTypeAsync(ComponentTypeDto dto)
    {
        return await _componentRepository.UpdateTypeAsync(dto);
    }
    public async Task<ServiceResponse<ComponentTypeDto>> RemoveComponentTypeAsync(int id)
    {
        return await _componentRepository.RemoveTypeAsync(id);
    }
    #endregion
    
    #region Issue Methods
    public async Task<ServiceResponse<IssueDto>> GetIssueAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        return await _issueRepository.GetAsync(filter);
    }

    public async Task<ServiceResponse<List<IssueDto>>> GetIssuesAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        return await _issueRepository.GetManyAsync(filter);
    }

    public async Task<ServiceResponse<IssueDto>> AddIssueAsync(IssueDto dto)
    {
        return await _issueRepository.AddAsync(dto);
    }

    public async Task<ServiceResponse<IssueDto>> UpdateIssueAsync(IssueDto dto)
    {
        return await _issueRepository.UpdateAsync(dto);
    }

    public async Task<ServiceResponse<IssueDto>> RemoveIssueAsync(int id)
    {
        return await _issueRepository.RemoveAsync(id);
    }
    
    public async Task<ServiceResponse<ComponentDto>> AssignIssueToComponent(int componentId, int issueId)
    {
        var componentResponse = await _componentRepository.GetAsync(e => e.Id == componentId);

        if (componentResponse.Result != ServiceResult.Success)
        {
            return componentResponse;
        }
        
        var issueResponse = await _issueRepository.GetAsync(i => i.Id == issueId);

        if (issueResponse.Result != ServiceResult.Success)
        {
            return new ServiceResponse<ComponentDto>
            {
                Result = issueResponse.Result,
                Payload = null,
                Message = issueResponse.Message
            };
        }

        componentResponse.Payload!.IssueId = issueId;

        var updatedComponentResponse = await _componentRepository.UpdateAsync(componentResponse.Payload);

        if (updatedComponentResponse.Result != ServiceResult.Success)
        {
            return updatedComponentResponse;
        }

        return await _componentRepository.GetAsync(e => e.Id == componentId);
    }

    #endregion
}