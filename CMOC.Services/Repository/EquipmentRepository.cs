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

        query = query.Include(e => e.Type)
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