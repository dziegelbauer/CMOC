using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class EquipmentRepository : AssetRepository<Equipment, EquipmentDto, EquipmentType, EquipmentTypeDto>, IEquipmentRepository
{
    public EquipmentRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<EquipmentDto?> GetAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.Relationships)
            .ThenInclude(ssr => ssr.Service)
            .Include(e => e.Components)
            .ThenInclude(cr => cr.Components);

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<EquipmentDto>();
    }

    public override async Task<List<EquipmentDto>> GetManyAsync(Expression<Func<Equipment, bool>>? filter = null)
    {
        var query = _db.Equipment.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(e => e.Type)
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

    public override async Task<EquipmentDto> AddAsync(EquipmentDto dto)
    {
        var equipment = await _db.Equipment.AddAsync(dto.Adapt<Equipment>());

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

    public override async Task<EquipmentDto> UpdateAsync(EquipmentDto dto)
    {
        var equipment = _db.Equipment.Update(dto.Adapt<Equipment>());

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

    public override async Task<bool> RemoveAsync(int id)
    {
        var equipmentInUse = _db.ServiceSupportRelationships.Any(ssr => ssr.EquipmentId == id);

        if (equipmentInUse)
        {
            return false;
        }
        
        return await DefaultRemoveAsync<Equipment>(_db, id);
    }
}