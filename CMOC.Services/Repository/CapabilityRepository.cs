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

    public async Task<ServiceResponse<CapabilityDto>> GetAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        var query = _db.Capabilities.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return new ServiceResponse<CapabilityDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Capability not found."
            };
        }

        return new ServiceResponse<CapabilityDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned capability."
        };
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

    public async Task<ServiceResponse<List<CapabilityDto>>> GetManyAsync(Expression<Func<Capability, bool>>? filter = null)
    {
        var query = _db.Capabilities.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.ToListAsync();

        return new ServiceResponse<List<CapabilityDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(c => c.ToDto())
                .ToList(),
            Message = "Successfully returned capabilities."
        };
    }

    public async Task<ServiceResponse<CapabilityDto>> AddAsync(CapabilityDto dto)
    {
        var newCapability = await _db.Capabilities.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(c => c.Id == newCapability.Entity.Id);
    }

    public async Task<ServiceResponse<CapabilityDto>> UpdateAsync(CapabilityDto dto)
    {
        var updatedCapability = _db.Capabilities.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(c => c.Id == updatedCapability.Entity.Id);
    }

    public async Task<ServiceResponse<CapabilityDto>> RemoveAsync(int id)
    {
        var capabilityInUse = _db.CapabilitySupportRelationships.Any(csr => csr.CapabilityId == id);

        if (capabilityInUse)
        {
            return new ServiceResponse<CapabilityDto>
            {
                Result = ServiceResult.InUse,
                Payload = null,
                Message = "Capability could not be deleted. Remove any dependencies related to it."
            };
        }

        return await DefaultRemoveAsync<Capability, CapabilityDto>(_db, id);
    }
}