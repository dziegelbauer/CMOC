using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class ServiceRepository : Repository<Service, ServiceDto>, IServiceRepository
{
    public ServiceRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<ServiceDto?> GetAsync(Expression<Func<Service, bool>>? filter = null)
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

        return queryResult?.Adapt<ServiceDto>();
    }

    public override async Task<List<ServiceDto>> GetManyAsync(Expression<Func<Service, bool>>? filter = null)
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

        return queryResult
            .Select(s => s.Adapt<ServiceDto>())
            .ToList();
    }

    public override async Task<ServiceDto> AddAsync(ServiceDto dto)
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

    public override async Task<ServiceDto> UpdateAsync(ServiceDto dto)
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
}