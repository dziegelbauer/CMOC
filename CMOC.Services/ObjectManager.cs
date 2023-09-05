using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;

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
    public async Task<CapabilityDto?> GetCapabilityAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        return await _capabilityRepository.GetAsync(filter);
    }

    public async Task<List<CapabilityDto>> GetCapabilitiesAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        return await _capabilityRepository.GetManyAsync(filter);
    }

    public async Task<CapabilityDto> AddCapabilityAsync(CapabilityDto dto)
    {
        return await _capabilityRepository.AddAsync(dto);
    }

    public async Task<CapabilityDto> UpdateCapabilityAsync(CapabilityDto dto)
    {
        return await _capabilityRepository.UpdateAsync(dto);
    }

    public async Task<bool> RemoveCapabilityAsync(int id)
    {
        return await _capabilityRepository.RemoveAsync(id);
    }

    #endregion

    #region Service Methods
    public async Task<ServiceDto?> GetServiceAsync(Expression<Func<Service, bool>>? filter = null)
    {
        return await _serviceRepository.GetAsync(filter);
    }

    public async Task<List<ServiceDto>> GetServicesAsync(Expression<Func<Service, bool>>? filter = null)
    {
        return await _serviceRepository.GetManyAsync(filter);
    }

    public async Task<ServiceDto> AddServiceAsync(ServiceDto dto)
    {
        return await _serviceRepository.AddAsync(dto);
    }

    public async Task<ServiceDto> UpdateServiceAsync(ServiceDto dto)
    {
        return await _serviceRepository.UpdateAsync(dto);
    }

    public async Task<bool> RemoveServiceAsync(int id)
    {
        return await _serviceRepository.RemoveAsync(id);
    }

    #endregion

    #region Location Methods
    public async Task<LocationDto?> GetLocationAsync(Expression<Func<Location, bool>>? filter = null)
    {
        return await _locationRepository.GetAsync(filter);
    }

    public async Task<List<LocationDto>> GetLocationsAsync(Expression<Func<Location, bool>>? filter = null)
    {
        return await _locationRepository.GetManyAsync(filter);
    }

    public async Task<LocationDto> AddLocationAsync(LocationDto dto)
    {
        return await _locationRepository.AddAsync(dto);
    }

    public async Task<LocationDto> UpdateLocationAsync(LocationDto dto)
    {
        return await _locationRepository.UpdateAsync(dto);
    }

    public async Task<bool> RemoveLocationAsync(int id)
    {
        return await _locationRepository.RemoveAsync(id);
    }

    #endregion

    #region Equipment Methods
    public async Task<EquipmentDto?> GetEquipmentItemAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        return await _equipmentRepository.GetAsync(filter);
    }

    public async Task<List<EquipmentDto>> GetEquipmentItemsAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        return await _equipmentRepository.GetManyAsync(filter);
    }

    public async Task<EquipmentDto> AddEquipmentItemAsync(EquipmentDto dto)
    {
        return await _equipmentRepository.AddAsync(dto);
    }

    public async Task<EquipmentDto> UpdateEquipmentItemAsync(EquipmentDto dto)
    {
        return await _equipmentRepository.UpdateAsync(dto);
    }

    public async Task<bool> RemoveEquipmentItemAsync(int id)
    {
        return await _equipmentRepository.RemoveAsync(id);
    }

    #endregion

    #region EquipmentType Methods
    public async Task<EquipmentTypeDto?> GetEquipmentTypeAsync(Expression<Func<EquipmentType, bool>>? filter = null)
    {
        return await _equipmentRepository.GetTypeAsync(filter);
    }

    public async Task<List<EquipmentTypeDto>> GetEquipmentTypesAsync(
        Expression<Func<EquipmentType, bool>>? filter = null)
    {
        return await _equipmentRepository.GetTypesAsync(filter);
    }

    public async Task<EquipmentTypeDto> AddEquipmentTypeAsync(EquipmentTypeDto dto)
    {
        return await _equipmentRepository.AddTypeAsync(dto);
    }

    public async Task<EquipmentTypeDto> UpdateEquipmentTypeAsync(EquipmentTypeDto dto)
    {
        return await _equipmentRepository.UpdateTypeAsync(dto);
    }

    public async Task<bool> RemoveEquipmentTypeAsync(int id)
    {
        return await _equipmentRepository.RemoveTypeAsync(id);
    }

    public async Task<EquipmentDto?> AssignIssueToEquipment(int equipmentId, int issueId)
    {
        var equipment = await _equipmentRepository.GetAsync(e => e.Id == equipmentId);

        if (equipment is null)
        {
            return null;
        }
        
        var issue = await _issueRepository.GetAsync(i => i.Id == issueId);

        if (issue is null)
        {
            return null;
        }

        equipment.IssueId = issueId;

        var updatedEquipment = await _equipmentRepository.UpdateAsync(equipment);

        if (updatedEquipment.IssueId is null)
        {
            return null;
        }

        return await _equipmentRepository.GetAsync(e => e.Id == equipmentId);
    }
    
    #endregion

    #region Component Methods

    public async Task<ComponentDto?> GetComponentAsync(Expression<Func<Component, bool>>? filter = null)
    {
        return await _componentRepository.GetAsync(filter);
    }

    public async Task<List<ComponentDto>> GetComponentsAsync(Expression<Func<Component, bool>>? filter = null)
    {
        return await _componentRepository.GetManyAsync(filter);
    }

    public async Task<ComponentDto> AddComponentAsync(ComponentDto dto)
    {
        return await _componentRepository.AddAsync(dto);
    }

    public async Task<ComponentDto> UpdateComponentAsync(ComponentDto dto)
    {
        return await _componentRepository.UpdateAsync(dto);
    }

    public async Task<bool> RemoveComponentAsync(int id)
    {
        return await _componentRepository.RemoveAsync(id);
    }

    #endregion

    #region ComponentType Methods
    public async Task<ComponentTypeDto?> GetComponentTypeAsync(Expression<Func<ComponentType, bool>>? filter = null)
    {
        return await _componentRepository.GetTypeAsync(filter);
    }
    public async Task<List<ComponentTypeDto>> GetComponentTypesAsync(Expression<Func<ComponentType, bool>>? filter = null)
    {
        return await _componentRepository.GetTypesAsync(filter);
    }
    public async Task<ComponentTypeDto> AddComponentTypeAsync(ComponentTypeDto dto)
    {
        return await _componentRepository.AddTypeAsync(dto);
    }

    public async Task<ComponentTypeDto> UpdateComponentTypeAsync(ComponentTypeDto dto)
    {
        return await _componentRepository.UpdateTypeAsync(dto);
    }
    public async Task<bool> RemoveComponentTypeAsync(int id)
    {
        return await _componentRepository.RemoveTypeAsync(id);
    }
    #endregion
    
    #region Issue Methods
    public async Task<IssueDto?> GetIssueAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        return await _issueRepository.GetAsync(filter);
    }

    public async Task<List<IssueDto>> GetIssuesAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        return await _issueRepository.GetManyAsync(filter);
    }

    public async Task<IssueDto> AddIssueAsync(IssueDto dto)
    {
        return await _issueRepository.AddAsync(dto);
    }

    public async Task<IssueDto> UpdateIssueAsync(IssueDto dto)
    {
        return await _issueRepository.UpdateAsync(dto);
    }

    public async Task<bool> RemoveIssueAsync(int id)
    {
        return await _issueRepository.RemoveAsync(id);
    }
    
    public async Task<ComponentDto?> AssignIssueToComponent(int componentId, int issueId)
    {
        var component = await _componentRepository.GetAsync(e => e.Id == componentId);

        if (component is null)
        {
            return null;
        }
        
        var issue = await _issueRepository.GetAsync(i => i.Id == issueId);

        if (issue is null)
        {
            return null;
        }

        component.IssueId = issueId;

        var updatedComponent = await _componentRepository.UpdateAsync(component);

        if (updatedComponent.IssueId is null)
        {
            return null;
        }

        return await _componentRepository.GetAsync(e => e.Id == componentId);
    }

    #endregion
}