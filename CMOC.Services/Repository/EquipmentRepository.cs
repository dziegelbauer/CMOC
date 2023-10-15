using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Utility;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class EquipmentRepository : Repository, IEquipmentRepository
{
    public EquipmentRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<EquipmentDto?> GetAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.ToDto();
    }

    private static IQueryable<Equipment> MapJoins(IQueryable<Equipment> query)
    {
        return query
            .Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.Location)
            .Include(e => e.Relationships)
            .ThenInclude(ssr => ssr.Service)
            .Include(e => e.Components)
            .ThenInclude(cr => cr.Components)
            .ThenInclude(c => c.Issue)
            .Include(e => e.Components)
            .ThenInclude(cr => cr.Components)
            .ThenInclude(c => c.Type);
    }

    public async Task<List<EquipmentDto>> GetManyAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.ToListAsync();
        return queryResult
            .Select(e => e.ToDto())
            .ToList();
    }

    public async Task<EquipmentDto> AddAsync(EquipmentDto dto)
    {
        var equipment = await _db.Equipment.AddAsync(dto.ToEntity());

        await _db.SaveChangesAsync();

        var newServiceSupportRelationships = dto.SupportedServices
            .Select(s => new ServiceSupportRelationship
            {
                EquipmentId = equipment.Entity.Id,
                FailureThreshold = 0,
                RedundantWithId = null,
                ServiceId = s,
                TypeId = dto.TypeId
            })
            .ToList();

        await _db.ServiceSupportRelationships.AddRangeAsync(newServiceSupportRelationships);

        await _db.SaveChangesAsync();
        return await GetAsync(e => e.Id == equipment.Entity.Id) ?? throw new Exception();
    }

    public async Task<EquipmentDto> UpdateAsync(EquipmentDto dto)
    {
        var equipment = _db.Equipment.Update(dto.ToEntity());

        var supportRelationships =
            await _db.ServiceSupportRelationships
                .Where(ssr => ssr.EquipmentId == dto.Id)
                .ToListAsync();

        supportRelationships
            .Where(ssr => !dto.SupportedServices.Select(s => s).Contains(ssr.ServiceId))
            .ToList()
            .ForEach(ssr => _db.ServiceSupportRelationships.Remove(ssr));

        foreach (var service in dto.SupportedServices)
        {
            var relationship = supportRelationships.FirstOrDefault(ssr => ssr.ServiceId == service);

            if (relationship is null)
            {
                await _db.ServiceSupportRelationships.AddAsync(new ServiceSupportRelationship
                {
                    ServiceId  = service,
                    EquipmentId = dto.Id,
                    RedundantWithId = null,
                    TypeId = dto.TypeId
                });
            }
        }

        await _db.SaveChangesAsync();

        return await GetAsync(e => e.Id == dto.Id) ?? throw new Exception();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var equipment = await _db.Equipment
            .Include(e => e.Components)
            .ThenInclude(cr => cr.Components)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipment is null)
        {
            return false;
        }

        equipment.Components.ForEach(cr =>
        {
            cr.Components.ForEach(c => _db.Remove(c));
            _db.Remove(cr);
        });
        
        var equipmentInUse = _db.ServiceSupportRelationships.Any(ssr => ssr.EquipmentId == id);

        if (equipmentInUse)
        {
            var affectedRelationships = await _db.ServiceSupportRelationships
                .Where(ssr => ssr.EquipmentId == id)
                .ToListAsync();

            _db.ServiceSupportRelationships.RemoveRange(affectedRelationships);
        }

        _db.Remove(equipment);
        await _db.SaveChangesAsync();
        
        return true;
    }

    public async Task<EquipmentTypeDto?> GetTypeAsync(Expression<Func<EquipmentType, bool>>? filter = null)
    {
        var query = _db.EquipmentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.ToDto();
    }

    public async Task<List<EquipmentTypeDto>> GetTypesAsync(Expression<Func<EquipmentType, bool>>? filter = null)
    {
        var query = _db.EquipmentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(et => et.ToDto())
            .ToList();
    }

    public async Task<EquipmentTypeDto> AddTypeAsync(EquipmentTypeDto dto)
    {
        var newEquipmentType = await _db.EquipmentTypes.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetTypeAsync(et => et.Id == newEquipmentType.Entity.Id) ?? throw new Exception();
    }

    public async Task<EquipmentTypeDto> UpdateTypeAsync(EquipmentTypeDto dto)
    {
        var updatedEquipmentType = _db.EquipmentTypes.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetTypeAsync(ct => ct.Id == updatedEquipmentType.Entity.Id) ?? throw new Exception();
    }

    public async Task<bool> RemoveTypeAsync(int id)
    {
        var typeInUse = _db.Equipment.Any(c => c.TypeId == id);

        if (typeInUse)
        {
            return false;
        }

        return await DefaultRemoveAsync<EquipmentType>(_db, id);
    }
}