using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Utility;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class CapabilityRepository : Repository, ICapabilityRepository
{
    public CapabilityRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<CapabilityDto?> GetAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        var query = _db.Capabilities.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.ToDto();
    }

    private static IQueryable<Capability> MapJoins(IQueryable<Capability> query)
    {
        return query
            .Include(c => c.SupportedBy)
            .ThenInclude(csr => csr.Service)
            .ThenInclude(s => s.SupportedBy)
            .ThenInclude(ssr => ssr.Equipment)
            .ThenInclude(e => e.Components)
            .ThenInclude(cr => cr.Components);
    }

    public async Task<List<CapabilityDto>> GetManyAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        var query = _db.Capabilities.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(c =>
            {
                var dto = c.ToDto();
                dto.Status = c.ParseStatusGraph();
                return dto;
            })
            .ToList();
    }

    public async Task<CapabilityDto> AddAsync(CapabilityDto dto)
    {
        var newCapability = await _db.Capabilities.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(c => c.Id == newCapability.Entity.Id) ?? throw new Exception();
    }

    public async Task<CapabilityDto> UpdateAsync(CapabilityDto dto)
    {
        var updatedCapability = _db.Capabilities.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(c => c.Id == updatedCapability.Entity.Id) ?? throw new Exception();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var capabilityInUse = _db.CapabilitySupportRelationships.Any(csr => csr.CapabilityId == id);

        if (capabilityInUse)
        {
            return false;
        }

        return await DefaultRemoveAsync<Capability>(_db, id);
    }
}