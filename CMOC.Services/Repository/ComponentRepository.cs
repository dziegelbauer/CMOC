using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Utility;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class ComponentRepository : Repository, IComponentRepository
{
    public ComponentRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<ServiceResponse<ComponentDto>> GetAsync(Expression<Func<Component, bool>>? filter = null)
    {
        var query = _db.Components.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.FirstOrDefaultAsync();
        
        if (queryResult is null)
        {
            return new ServiceResponse<ComponentDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Component not found."
            };
        }

        return new ServiceResponse<ComponentDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned component."
        };
    }

    private static IQueryable<Component> MapJoins(IQueryable<Component> query)
    {
        query = query
            .Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.ComponentOf)
            .ThenInclude(cr => cr.Equipment)
            .ThenInclude(e => e.Type);
        return query;
    }

    public async Task<ServiceResponse<List<ComponentDto>>> GetManyAsync(Expression<Func<Component, bool>>? filter = null)
    {
        var query = _db.Components.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.ToListAsync();

        return new ServiceResponse<List<ComponentDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(c => c.ToDto())
                .ToList(),
            Message = "Successfully returned components."
        };
    }

    public async Task<ServiceResponse<ComponentDto>> AddAsync(ComponentDto dto)
    {
        var componentRelationship =
            await _db.ComponentRelationships.FirstOrDefaultAsync(cr =>
                cr.TypeId == dto.TypeId && cr.EquipmentId == dto.EquipmentId) 
            ?? (await _db.ComponentRelationships.AddAsync(new ComponentRelationship
            {
                EquipmentId = dto.EquipmentId,
                FailureThreshold = 1,
                TypeId = dto.TypeId
            })).Entity;

        if (componentRelationship.Id != 0)
        {
            componentRelationship.FailureThreshold++;
        }

        var component = dto.ToEntity();
        component.ComponentOf = componentRelationship;
        await _db.Components.AddAsync(component);
        await _db.SaveChangesAsync();
        return await GetAsync(c => c.Id == component.Id);
    }

    public async Task<ServiceResponse<ComponentDto>> UpdateAsync(ComponentDto dto)
    {
        var component = await _db.Components.FindAsync(dto.Id);

        if (component is null)
        {
            throw new Exception();
        }

        component.Operational = dto.Operational;
        component.SerialNumber = dto.SerialNumber;
        component.TypeId = dto.TypeId;
        component.IssueId = dto.IssueId;
        
        var oldRelationship = await _db.ComponentRelationships.FindAsync(component.ComponentOfId);
        
        var componentRelationship =
            await _db.ComponentRelationships.FirstOrDefaultAsync(cr =>
                cr.TypeId == dto.TypeId && cr.EquipmentId == dto.EquipmentId);

        if (componentRelationship is not null)
        {
            if (componentRelationship.Id != component.ComponentOfId)
            {
                component.ComponentOfId = componentRelationship.Id;
                componentRelationship.FailureThreshold++;
            }
        }
        else
        {
            var newRelationship = (await _db.ComponentRelationships.AddAsync(new ComponentRelationship
            {
                EquipmentId = dto.EquipmentId,
                FailureThreshold = 1,
                TypeId = dto.TypeId
            })).Entity;

            component.ComponentOf = newRelationship;
        }

        if (oldRelationship is not null && oldRelationship.Id != component.ComponentOfId)
        {
            if (!await _db.Components
                    .Where(c => c.Id != component.Id && c.ComponentOfId == oldRelationship.Id)
                    .AnyAsync())
            {
                _db.ComponentRelationships.Remove(oldRelationship);
            }
            else
            {
                oldRelationship.FailureThreshold--;
            }
        }
        
        _db.Components.Update(component);
        await _db.SaveChangesAsync();
        
        return await GetAsync(c => c.Id == dto.Id);
    }

    public async Task<ServiceResponse<ComponentDto>> RemoveAsync(int id)
    {
        var component = await _db.Components.FindAsync(id);

        if (component is null)
        {
            return new ServiceResponse<ComponentDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Service not found."
            };
        }

        var componentInUse = _db.ComponentRelationships.Any(cr => cr.Id == component.ComponentOfId);

        if (componentInUse)
        {
            var affectedRelationships = await _db.ComponentRelationships
                .Where(cr => cr.Id == component.ComponentOfId)
                .ToListAsync();

            _db.ComponentRelationships.RemoveRange(affectedRelationships);
        }

        _db.Components.Remove(component);

        await _db.SaveChangesAsync();
        
        return new ServiceResponse<ComponentDto>
        {
            Result = ServiceResult.Success,
            Payload = null,
            Message = "component deleted successfully."
        };            
    }

    public async Task<ServiceResponse<ComponentTypeDto>> GetTypeAsync(Expression<Func<ComponentType, bool>>? filter = null)
    {
        var query = _db.ComponentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return new ServiceResponse<ComponentTypeDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Component type not found."
            };
        }

        return new ServiceResponse<ComponentTypeDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned component type."
        };
    }

    public async Task<ServiceResponse<List<ComponentTypeDto>>> GetTypesAsync(Expression<Func<ComponentType, bool>>? filter = null)
    {
        var query = _db.ComponentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return new ServiceResponse<List<ComponentTypeDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(ct => ct.ToDto())
                .ToList(),
            Message = "Successfully returned component types."
        };
    }

    public async Task<ServiceResponse<ComponentTypeDto>> AddTypeAsync(ComponentTypeDto dto)
    {
        var newComponentType = await _db.ComponentTypes.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetTypeAsync(ct => ct.Id == newComponentType.Entity.Id);
    }

    public async Task<ServiceResponse<ComponentTypeDto>> UpdateTypeAsync(ComponentTypeDto dto)
    {
        var updatedComponentType = _db.ComponentTypes.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetTypeAsync(ct => ct.Id == updatedComponentType.Entity.Id);
    }

    public async Task<ServiceResponse<ComponentTypeDto>> RemoveTypeAsync(int id)
    {
        var typeInUse = _db.Components.Any(c => c.TypeId == id);

        if (typeInUse)
        {
            return new ServiceResponse<ComponentTypeDto>
            {
                Result = ServiceResult.InUse,
                Payload = null,
                Message = "Capability type could not be deleted. Remove any dependencies related to it."
            };
        }

        return await DefaultRemoveAsync<ComponentType, ComponentTypeDto>(_db, id);
    }
}