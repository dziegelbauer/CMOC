using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class CapabilityRepository : Repository<Capability, CapabilityDto>, ICapabilityRepository
{
    public CapabilityRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<CapabilityDto?> GetAsync(Expression<Func<Capability, bool>>? filter = null)
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

        return queryResult?.Adapt<CapabilityDto>();
    }

    public override async Task<List<CapabilityDto>> GetManyAsync(Expression<Func<Capability, bool>>? filter = null)
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

        return queryResult
            .Select(c => c.Adapt<CapabilityDto>())
            .ToList();
    }
}