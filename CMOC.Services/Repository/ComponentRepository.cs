using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class ComponentRepository : AssetRepository<Component, ComponentDto, ComponentType, ComponentTypeDto>, IComponentRepository
{
    public ComponentRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<ComponentDto?> GetAsync(Expression<Func<Component, bool>>? filter = null)
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
            .ThenInclude(cr => cr.Equipment)
            .ThenInclude(e => e.Type);

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<ComponentDto>();
    }

    public override async Task<List<ComponentDto>> GetManyAsync(Expression<Func<Component, bool>>? filter = null)
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
            .ThenInclude(cr => cr.Equipment)
            .ThenInclude(e => e.Type);

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(c => c.Adapt<ComponentDto>())
            .ToList();
    }

    public override async Task<ComponentDto> AddAsync(ComponentDto dto)
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

        var component = dto.Adapt<Component>();
        component.ComponentOf = componentRelationship;
        await _db.Components.AddAsync(component);
        await _db.SaveChangesAsync();
        return await GetAsync(c => c.Id == component.Id) ?? throw new Exception();
    }

    public override async Task<ComponentDto> UpdateAsync(ComponentDto dto)
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
        
        return await GetAsync(c => c.Id == dto.Id) ?? throw new Exception();
    }

    public override async Task<bool> RemoveAsync(int id)
    {
        var component = await _db.Components.FindAsync(id);

        if (component is null)
        {
            return false;
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
        
        return true;
    }
}