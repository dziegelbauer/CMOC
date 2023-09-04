using System.Linq.Expressions;
using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services;

public interface IObjectManager
{
    #region Capability Methods
    Task<CapabilityDto?> GetCapabilityAsync(Expression<Func<Capability,bool>>? filter = null);
    Task<List<CapabilityDto>> GetCapabilitiesAsync(Expression<Func<Capability,bool>>? filter = null);
    Task<CapabilityDto> AddCapabilityAsync(CapabilityDto dto);
    Task<CapabilityDto> UpdateCapabilityAsync(CapabilityDto dto);
    Task<bool> RemoveCapabilityAsync(int id);
    #endregion

    #region Service Methods
    Task<ServiceDto?> GetServiceAsync(Expression<Func<Service,bool>>? filter = null);
    Task<List<ServiceDto>> GetServicesAsync(Expression<Func<Service,bool>>? filter = null);
    Task<ServiceDto> AddServiceAsync(ServiceDto dto);
    Task<ServiceDto> UpdateServiceAsync(ServiceDto dto);
    Task<bool> RemoveServiceAsync(int id);
    #endregion
    
    #region Location Methods
    Task<LocationDto?> GetLocationAsync(Expression<Func<Location,bool>>? filter = null);
    Task<List<LocationDto>> GetLocationsAsync(Expression<Func<Location,bool>>? filter = null);
    Task<LocationDto> AddLocationAsync(LocationDto dto);
    Task<LocationDto> UpdateLocationAsync(LocationDto dto);
    Task<bool> RemoveLocationAsync(int id);
    #endregion
    
    #region Equipment Methods
    Task<EquipmentDto?> GetEquipmentItemAsync(Expression<Func<Equipment,bool>>? filter = null);
    Task<List<EquipmentDto>> GetEquipmentItemsAsync(Expression<Func<Equipment,bool>>? filter = null);
    Task<EquipmentDto> AddEquipmentItemAsync(EquipmentDto dto);
    Task<EquipmentDto> UpdateEquipmentItemAsync(EquipmentDto dto);
    Task<bool> RemoveEquipmentItemAsync(int id);
    #endregion

    #region EquipmentType Methods
    Task<EquipmentTypeDto?> GetEquipmentTypeAsync(Expression<Func<EquipmentType, bool>>? filter = null);
    Task<List<EquipmentTypeDto>> GetEquipmentTypesAsync(Expression<Func<EquipmentType, bool>>? filter = null);
    Task<EquipmentTypeDto> AddEquipmentTypeAsync(EquipmentTypeDto dto);
    Task<EquipmentTypeDto> UpdateEquipmentTypeAsync(EquipmentTypeDto dto);
    Task<bool> RemoveEquipmentTypeAsync(int id);
    #endregion

    #region Component Methods
    Task<ComponentDto?> GetComponentAsync(Expression<Func<Component, bool>>? filter = null);
    Task<List<ComponentDto>> GetComponentsAsync(Expression<Func<Component, bool>>? filter = null);
    Task<ComponentDto> AddComponentAsync(ComponentDto dto);
    Task<ComponentDto> UpdateComponentAsync(ComponentDto dto);
    Task<bool> RemoveComponentAsync(int id);
    #endregion

    #region ComponentType Methods
    Task<ComponentTypeDto?> GetComponentTypeAsync(Expression<Func<ComponentType, bool>>? filter = null);
    Task<List<ComponentTypeDto>> GetComponentTypesAsync(Expression<Func<ComponentType, bool>>? filter = null);
    Task<ComponentTypeDto> AddComponentTypeAsync(ComponentTypeDto dto);
    Task<ComponentTypeDto> UpdateComponentTypeAsync(ComponentTypeDto dto);
    Task<bool> RemoveComponentTypeAsync(int id);
    #endregion
    
    #region Issue Methods
    Task<IssueDto?> GetIssueAsync(Expression<Func<Issue, bool>>? filter = null);
    Task<List<IssueDto>> GetIssuesAsync(Expression<Func<Issue, bool>>? filter = null);
    Task<IssueDto> AddIssueAsync(IssueDto dto);
    Task<IssueDto> UpdateIssueAsync(IssueDto dto);
    Task<bool> RemoveIssueAsync(int id);
    #endregion
}