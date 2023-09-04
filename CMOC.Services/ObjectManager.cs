using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services;

public class ObjectManager : IObjectManager
{
    private readonly AppDbContext _db;

    public ObjectManager(AppDbContext db)
    {
        _db = db;
    }

    #region Capability Methods

    public async Task<CapabilityDto?> GetCapabilityAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        var query = _db.Capabilities.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(c => c.SupportedBy)
            .ThenInclude(csr => csr.Service);

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return null;
        }

        return new CapabilityDto
        {
            Id = queryResult.Id,
            Name = queryResult.Name,
            Dependencies = queryResult.SupportedBy
                .Select(csr => csr.Service)
                .Select(s => s.Adapt<ServiceDto>())
                .ToList()
        };
    }

    public async Task<List<CapabilityDto>> GetCapabilitiesAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        var query = _db.Capabilities.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(c => c.SupportedBy)
            .ThenInclude(csr => csr.Service);

        var queryResult = await query.ToListAsync();

        return queryResult.Select(c =>
            {
                var dto = new CapabilityDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Dependencies = c.SupportedBy
                        .Select(csr => csr.Service)
                        .Select(s => s.Adapt<ServiceDto>())
                        .ToList()
                };

                return dto;
            })
            .ToList();
    }

    public async Task<CapabilityDto> AddCapabilityAsync(CapabilityDto dto)
    {
        var capability = await _db.Capabilities.AddAsync(dto.Adapt<Capability>());
        await _db.SaveChangesAsync();
        return capability.Entity.Adapt<CapabilityDto>();
    }

    public async Task<CapabilityDto> UpdateCapabilityAsync(CapabilityDto dto)
    {
        var capability = _db.Capabilities.Update(dto.Adapt<Capability>());
        await _db.SaveChangesAsync();
        return capability.Entity.Adapt<CapabilityDto>();
    }

    public async Task<bool> RemoveCapabilityAsync(int id)
    {
        var objFromDb = await _db.Capabilities.FirstOrDefaultAsync(c => c.Id == id);

        if (objFromDb is null)
        {
            return false;
        }

        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Service Methods
    public async Task<ServiceDto?> GetServiceAsync(Expression<Func<Service, bool>>? filter = null)
    {
        var query = _db.Services.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(s => s.SupportedBy)
            .ThenInclude(ssr => ssr.Equipment)
            .ThenInclude(e => e.Components)
            .ThenInclude(cr => cr.Components)
            .Include(s => s.Supports)
            .ThenInclude(csr => csr.Capability);

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return null;
        }

        return new ServiceDto
        {
            Id = queryResult.Id,
            Name = queryResult.Name,
            Dependencies = queryResult.SupportedBy
                .Select(ssr => ssr.Equipment)
                .Select(e => e.Adapt<EquipmentDto>())
                .ToList(),
            Dependents = queryResult.Supports
                .Select(csr => csr.Capability)
                .Select(c => c.Id)
                .ToList()
        };
    }

    public async Task<List<ServiceDto>> GetServicesAsync(Expression<Func<Service, bool>>? filter = null)
    {
        var query = _db.Services.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(s => s.SupportedBy)
            .ThenInclude(ssr => ssr.Equipment)
            .Include(s => s.Supports)
            .ThenInclude(csr => csr.Capability);

        var queryResult = await query.ToListAsync();

        return queryResult.Select(s =>
            {
                var dto = new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Dependencies = s.SupportedBy
                        .Select(ssr => ssr.Equipment)
                        .Select(e => e.Adapt<EquipmentDto>())
                        .ToList(),
                    Dependents = s.Supports
                        .Select(csr => csr.Capability)
                        .Select(c => c.Id)
                        .ToList()
                };

                return dto;
            })
            .ToList();
    }

    public async Task<ServiceDto> AddServiceAsync(ServiceDto dto)
    {
        var service = await _db.Services.AddAsync(dto.Adapt<Service>());

        await _db.SaveChangesAsync();

        foreach (var capability in dto.Dependents)
        {
            await _db.CapabilitySupportRelationships.AddAsync(new CapabilitySupportRelationship
            {
                CapabilityId = capability,
                ServiceId = service.Entity.Id,
                RedundantWithId = null
            });
        }

        await _db.SaveChangesAsync();
        return service.Entity.Adapt<ServiceDto>();
    }

    public async Task<ServiceDto> UpdateServiceAsync(ServiceDto dto)
    {
        var service = _db.Services.Update(dto.Adapt<Service>());

        var supportRelationships =
            await _db.CapabilitySupportRelationships
                .Where(csr => csr.ServiceId == dto.Id)
                .ToListAsync();

        supportRelationships
            .Where(csr => !dto.Dependents.Select(c => c).Contains(csr.CapabilityId))
            .ToList()
            .ForEach(csr => _db.CapabilitySupportRelationships.Remove(csr));

        foreach (var capability in dto.Dependents)
        {
            var relationship = supportRelationships.FirstOrDefault(sr => sr.CapabilityId == capability);

            if (relationship is null)
            {
                await _db.CapabilitySupportRelationships.AddAsync(new CapabilitySupportRelationship
                {
                    CapabilityId = capability,
                    ServiceId = dto.Id,
                    RedundantWithId = null
                });
            }
        }

        await _db.SaveChangesAsync();

        return service.Entity.Adapt<ServiceDto>();
    }

    public async Task<bool> RemoveServiceAsync(int id)
    {
        var objFromDb = await _db.Capabilities.FirstOrDefaultAsync(c => c.Id == id);

        if (objFromDb is null)
        {
            return false;
        }

        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Location Methods

    public async Task<LocationDto?> GetLocationAsync(Expression<Func<Location, bool>>? filter = null)
    {
        var query = _db.Locations.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<LocationDto>();
    }

    public async Task<List<LocationDto>> GetLocationsAsync(Expression<Func<Location, bool>>? filter = null)
    {
        var query = _db.Locations.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(l => l.Adapt<LocationDto>())
            .ToList();
    }

    public async Task<LocationDto> AddLocationAsync(LocationDto dto)
    {
        var location = await _db.Locations.AddAsync(dto.Adapt<Location>());
        await _db.SaveChangesAsync();
        return location.Entity.Adapt<LocationDto>();
    }

    public async Task<LocationDto> UpdateLocationAsync(LocationDto dto)
    {
        var location = _db.Locations.Update(dto.Adapt<Location>());
        await _db.SaveChangesAsync();
        return location.Entity.Adapt<LocationDto>();
    }

    public async Task<bool> RemoveLocationAsync(int id)
    {
        var objFromDb = await _db.Locations.FirstOrDefaultAsync(l => l.Id == id);

        if (objFromDb is null)
        {
            return false;
        }

        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Equipment Methods

    public async Task<EquipmentDto?> GetEquipmentItemAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query.Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.Relationships)
            .ThenInclude(ssr => ssr.Service)
            .Include(e => e.Components)
            .ThenInclude(cr => cr.Components);

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<EquipmentDto>();
    }

    public async Task<List<EquipmentDto>> GetEquipmentItemsAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query.Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.Relationships)
            .ThenInclude(ssr => ssr.Service)
            .Include(e => e.Components)
            .ThenInclude(cr => cr.Components);

        var queryResult = await query.ToListAsync();
        return queryResult
            .Select(e => e.Adapt<EquipmentDto>())
            .ToList();
    }

    public async Task<EquipmentDto> AddEquipmentItemAsync(EquipmentDto dto)
    {
        var equipment = await _db.Equipment.AddAsync(dto.Adapt<Equipment>());
        await _db.SaveChangesAsync();
        return new EquipmentDto
        {
            Id = equipment.Entity.Id,
            LocationId = equipment.Entity.LocationId,
            Notes = equipment.Entity.Notes,
            OperationalOverride = equipment.Entity.OperationalOverride,
            SerialNumber = equipment.Entity.SerialNumber,
            TypeId = equipment.Entity.TypeId
        };
    }

    public async Task<EquipmentDto> UpdateEquipmentItemAsync(EquipmentDto dto)
    {
        var equipment = _db.Equipment.Update(dto.Adapt<Equipment>());
        await _db.SaveChangesAsync();
        return equipment.Entity.Adapt<EquipmentDto>();
    }

    public async Task<bool> RemoveEquipmentItemAsync(int id)
    {
        var objFromDb = await _db.Equipment.FirstOrDefaultAsync(e => e.Id == id);

        if (objFromDb is null)
        {
            return false;
        }

        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return true;
    }

    #endregion

    #region EquipmentType Methods

    public async Task<EquipmentTypeDto?> GetEquipmentTypeAsync(Expression<Func<EquipmentType, bool>>? filter = null)
    {
        var query = _db.EquipmentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<EquipmentTypeDto>();
    }

    public async Task<List<EquipmentTypeDto>> GetEquipmentTypesAsync(
        Expression<Func<EquipmentType, bool>>? filter = null)
    {
        var query = _db.EquipmentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(e => e.Adapt<EquipmentTypeDto>())
            .ToList();
    }

    public async Task<EquipmentTypeDto> AddEquipmentTypeAsync(EquipmentTypeDto dto)
    {
        var equipmentType = await _db.EquipmentTypes.AddAsync(dto.Adapt<EquipmentType>());
        await _db.SaveChangesAsync();
        return equipmentType.Entity.Adapt<EquipmentTypeDto>();
    }

    public async Task<EquipmentTypeDto> UpdateEquipmentTypeAsync(EquipmentTypeDto dto)
    {
        var equipmentType = _db.EquipmentTypes.Update(dto.Adapt<EquipmentType>());
        await _db.SaveChangesAsync();
        return equipmentType.Entity.Adapt<EquipmentTypeDto>();
    }

    public async Task<bool> RemoveEquipmentTypeAsync(int id)
    {
        var objFromDb = await _db.EquipmentTypes.FirstOrDefaultAsync(e => e.Id == id);

        if (objFromDb is null)
        {
            return false;
        }

        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Component Methods

    public async Task<ComponentDto?> GetComponentAsync(Expression<Func<Component, bool>>? filter = null)
    {
        var query = _db.Components.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.ComponentOf)
            .ThenInclude(cr => cr.Equipment);

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<ComponentDto>();
    }

    public async Task<List<ComponentDto>> GetComponentsAsync(Expression<Func<Component, bool>>? filter = null)
    {
        var query = _db.Components.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.ComponentOf)
            .ThenInclude(cr => cr.Equipment);

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(c => c.Adapt<ComponentDto>())
            .ToList();
    }

    public async Task<ComponentDto> AddComponentAsync(ComponentDto dto)
    {
        var component = await _db.Components.AddAsync(dto.Adapt<Component>());
        await _db.SaveChangesAsync();
        return component.Entity.Adapt<ComponentDto>();
    }

    public async Task<ComponentDto> UpdateComponentAsync(ComponentDto dto)
    {
        var component = _db.Components.Update(dto.Adapt<Component>());
        await _db.SaveChangesAsync();
        return component.Entity.Adapt<ComponentDto>();
    }

    public async Task<bool> RemoveComponentAsync(int id)
    {
        var objFromDb = await _db.Components.FirstOrDefaultAsync(c => c.Id == id);

        if (objFromDb is null)
        {
            return false;
        }

        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Issue Methods

    public async Task<IssueDto?> GetIssueAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        var query = _db.Issues.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return null;
        }

        return queryResult.Adapt<IssueDto>();
    }

    public async Task<List<IssueDto>> GetIssuesAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        var query = _db.Issues.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(l => l.Adapt<IssueDto>())
            .ToList();
    }

    public async Task<IssueDto> AddIssueAsync(IssueDto dto)
    {
        var issue = await _db.Issues.AddAsync(dto.Adapt<Issue>());
        await _db.SaveChangesAsync();
        return issue.Entity.Adapt<IssueDto>();
    }

    public async Task<IssueDto> UpdateIssueAsync(IssueDto dto)
    {
        var issue = _db.Issues.Update(dto.Adapt<Issue>());
        await _db.SaveChangesAsync();
        return issue.Entity.Adapt<IssueDto>();
    }

    public async Task<bool> RemoveIssueAsync(int id)
    {
        var objFromDb = await _db.Issues.FirstOrDefaultAsync(l => l.Id == id);

        if (objFromDb is null)
        {
            return false;
        }

        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return true;
    }

    #endregion
}