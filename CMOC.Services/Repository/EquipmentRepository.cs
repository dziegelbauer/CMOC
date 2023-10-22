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

    public async Task<ServiceResponse<EquipmentDto>> GetAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return new ServiceResponse<EquipmentDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Equipment item not found."
            };
        }

        return new ServiceResponse<EquipmentDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned equipment item."
        };
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

    public async Task<ServiceResponse<List<EquipmentDto>>> GetManyAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.ToListAsync();
        
        return new ServiceResponse<List<EquipmentDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(c => c.ToDto())
                .ToList(),
            Message = "Successfully returned equipment items."
        };
    }

    public async Task<ServiceResponse<EquipmentDto>> AddAsync(EquipmentDto dto)
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
        return await GetAsync(e => e.Id == equipment.Entity.Id);
    }

    public async Task<ServiceResponse<EquipmentDto>> UpdateAsync(EquipmentDto dto)
    {
        var equipment = _db.Equipment.Update(dto.ToEntity());

        var supportRelationships =
            await _db.ServiceSupportRelationships
                .Where(ssr => ssr.EquipmentId == equipment.Entity.Id)
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

        return await GetAsync(e => e.Id == dto.Id);
    }

    public async Task<ServiceResponse<EquipmentDto>> RemoveAsync(int id)
    {
        var equipment = await _db.Equipment
            .Include(e => e.Components)
            .ThenInclude(cr => cr.Components)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipment is null)
        {
            return new ServiceResponse<EquipmentDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Equipment item not found."
            };
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
        
        return new ServiceResponse<EquipmentDto>
        {
            Result = ServiceResult.Success,
            Payload = null,
            Message = "Equipment item deleted successfully."
        };
    }

    public async Task<ServiceResponse<EquipmentTypeDto>> GetTypeAsync(Expression<Func<EquipmentType, bool>>? filter = null)
    {
        var query = _db.EquipmentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return new ServiceResponse<EquipmentTypeDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Equipment type not found."
            };
        }

        return new ServiceResponse<EquipmentTypeDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned equipment type."
        };
    }

    public async Task<ServiceResponse<List<EquipmentTypeDto>>> GetTypesAsync(Expression<Func<EquipmentType, bool>>? filter = null)
    {
        var query = _db.EquipmentTypes.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return new ServiceResponse<List<EquipmentTypeDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(et => et.ToDto())
                .ToList(),
            Message = "Successfully returned component types."
        };
    }

    public async Task<ServiceResponse<EquipmentTypeDto>> AddTypeAsync(EquipmentTypeDto dto)
    {
        var newEquipmentType = await _db.EquipmentTypes.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetTypeAsync(et => et.Id == newEquipmentType.Entity.Id);
    }

    public async Task<ServiceResponse<EquipmentTypeDto>> UpdateTypeAsync(EquipmentTypeDto dto)
    {
        var updatedEquipmentType = _db.EquipmentTypes.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetTypeAsync(ct => ct.Id == updatedEquipmentType.Entity.Id);
    }

    public async Task<ServiceResponse<EquipmentTypeDto>> RemoveTypeAsync(int id)
    {
        var typeInUse = _db.Equipment.Any(c => c.TypeId == id);

        if (typeInUse)
        {
            return new ServiceResponse<EquipmentTypeDto>
            {
                Result = ServiceResult.InUse,
                Payload = null,
                Message = "Equipment type could not be deleted. Remove any dependencies related to it."
            };
        }

        return await DefaultRemoveAsync<EquipmentType, EquipmentTypeDto>(_db, id);
    }
}