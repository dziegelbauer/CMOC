using System.Linq.Expressions;
using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services;

public interface IObjectManager
{
    #region Capability Methods
    Task<ServiceResponse<CapabilityDto>> GetCapabilityAsync(Expression<Func<Capability,bool>>? filter = null);
    Task<ServiceResponse<List<CapabilityDto>>> GetCapabilitiesAsync(Expression<Func<Capability,bool>>? filter = null);
    Task<ServiceResponse<CapabilityDto>> AddCapabilityAsync(CapabilityDto dto);
    Task<ServiceResponse<CapabilityDto>> UpdateCapabilityAsync(CapabilityDto dto);
    Task<ServiceResponse<CapabilityDto>> RemoveCapabilityAsync(int id);
    #endregion

    #region Service Methods
    Task<ServiceResponse<ServiceDto>> GetServiceAsync(Expression<Func<Service,bool>>? filter = null);
    Task<ServiceResponse<List<ServiceDto>>> GetServicesAsync(Expression<Func<Service,bool>>? filter = null);
    Task<ServiceResponse<ServiceDto>> AddServiceAsync(ServiceDto dto);
    Task<ServiceResponse<ServiceDto>> UpdateServiceAsync(ServiceDto dto);
    Task<ServiceResponse<ServiceDto>> RemoveServiceAsync(int id);
    #endregion
    
    #region Location Methods
    Task<ServiceResponse<LocationDto>> GetLocationAsync(Expression<Func<Location,bool>>? filter = null);
    Task<ServiceResponse<List<LocationDto>>> GetLocationsAsync(Expression<Func<Location,bool>>? filter = null);
    Task<ServiceResponse<LocationDto>> AddLocationAsync(LocationDto dto);
    Task<ServiceResponse<LocationDto>> UpdateLocationAsync(LocationDto dto);
    Task<ServiceResponse<LocationDto>> RemoveLocationAsync(int id);
    #endregion
    
    #region Equipment Methods
    Task<ServiceResponse<EquipmentDto>> GetEquipmentItemAsync(Expression<Func<Equipment,bool>>? filter = null);
    Task<ServiceResponse<List<EquipmentDto>>> GetEquipmentItemsAsync(Expression<Func<Equipment,bool>>? filter = null);
    Task<ServiceResponse<EquipmentDto>> AddEquipmentItemAsync(EquipmentDto dto);
    Task<ServiceResponse<EquipmentDto>> UpdateEquipmentItemAsync(EquipmentDto dto);
    Task<ServiceResponse<EquipmentDto>> RemoveEquipmentItemAsync(int id);
    Task<ServiceResponse<EquipmentDto>> AssignIssueToEquipment(int equipmentId, int issueId);
    #endregion

    #region EquipmentType Methods
    Task<ServiceResponse<EquipmentTypeDto>> GetEquipmentTypeAsync(Expression<Func<EquipmentType, bool>>? filter = null);
    Task<ServiceResponse<List<EquipmentTypeDto>>> GetEquipmentTypesAsync(Expression<Func<EquipmentType, bool>>? filter = null);
    Task<ServiceResponse<EquipmentTypeDto>> AddEquipmentTypeAsync(EquipmentTypeDto dto);
    Task<ServiceResponse<EquipmentTypeDto>> UpdateEquipmentTypeAsync(EquipmentTypeDto dto);
    Task<ServiceResponse<EquipmentTypeDto>> RemoveEquipmentTypeAsync(int id);
    #endregion

    #region Component Methods
    Task<ServiceResponse<ComponentDto>> GetComponentAsync(Expression<Func<Component, bool>>? filter = null);
    Task<ServiceResponse<List<ComponentDto>>> GetComponentsAsync(Expression<Func<Component, bool>>? filter = null);
    Task<ServiceResponse<ComponentDto>> AddComponentAsync(ComponentDto dto);
    Task<ServiceResponse<ComponentDto>> UpdateComponentAsync(ComponentDto dto);
    Task<ServiceResponse<ComponentDto>> RemoveComponentAsync(int id);
    Task<ServiceResponse<ComponentDto>> AssignIssueToComponent(int componentId, int issueId);
    #endregion

    #region ComponentType Methods
    Task<ServiceResponse<ComponentTypeDto>> GetComponentTypeAsync(Expression<Func<ComponentType, bool>>? filter = null);
    Task<ServiceResponse<List<ComponentTypeDto>>> GetComponentTypesAsync(Expression<Func<ComponentType, bool>>? filter = null);
    Task<ServiceResponse<ComponentTypeDto>> AddComponentTypeAsync(ComponentTypeDto dto);
    Task<ServiceResponse<ComponentTypeDto>> UpdateComponentTypeAsync(ComponentTypeDto dto);
    Task<ServiceResponse<ComponentTypeDto>> RemoveComponentTypeAsync(int id);
    #endregion
    
    #region Issue Methods
    Task<ServiceResponse<IssueDto>> GetIssueAsync(Expression<Func<Issue, bool>>? filter = null);
    Task<ServiceResponse<List<IssueDto>>> GetIssuesAsync(Expression<Func<Issue, bool>>? filter = null);
    Task<ServiceResponse<IssueDto>> AddIssueAsync(IssueDto dto);
    Task<ServiceResponse<IssueDto>> UpdateIssueAsync(IssueDto dto);
    Task<ServiceResponse<IssueDto>> RemoveIssueAsync(int id);
    #endregion
}