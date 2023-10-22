using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Utility;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class ServiceRepository : Repository, IServiceRepository
{
    public ServiceRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<ServiceResponse<ServiceDto>> GetAsync(Expression<Func<Service, bool>>? filter = null)
    {
        var query = _db.Services.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return new ServiceResponse<ServiceDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Service not found."
            };
        }

        return new ServiceResponse<ServiceDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned service."
        };
    }

    public async Task<ServiceResponse<List<ServiceDto>>> GetManyAsync(Expression<Func<Service, bool>>? filter = null)
    {
        var query = _db.Services.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = MapJoins(query);

        var queryResult = await query.ToListAsync();

        return new ServiceResponse<List<ServiceDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(s => s.ToDto())
                .ToList(),
            Message = "Successfully returned services."
        };
    }

    private static IQueryable<Service> MapJoins(IQueryable<Service> query)
    {
        return query
            .Include(s => s.SupportedBy)
            .ThenInclude(ssr => ssr.Equipment)
            .ThenInclude(e => e.Components)
            .ThenInclude(cr => cr.Components)
            .Include(s => s.Supports)
            .ThenInclude(csr => csr.Capability);
    }

    public async Task<ServiceResponse<ServiceDto>> AddAsync(ServiceDto dto)
    {
        var service = await _db.Services.AddAsync(dto.ToEntity());

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
        return await GetAsync(s => s.Id == service.Entity.Id);
    }

    public async Task<ServiceResponse<ServiceDto>> UpdateAsync(ServiceDto dto)
    {
        var service = _db.Services.Update(dto.ToEntity());

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

        return await GetAsync(s => s.Id == service.Entity.Id);
    }

    public async Task<ServiceResponse<ServiceDto>> RemoveAsync(int id)
    {
        var serviceInUse = _db.ServiceSupportRelationships.Any(ssr => ssr.ServiceId == id)
                           || _db.CapabilitySupportRelationships.Any(csr => csr.ServiceId == id);

        if (serviceInUse)
        {
            return new ServiceResponse<ServiceDto>
            {
                Result = ServiceResult.InUse,
                Payload = null,
                Message = "Service could not be deleted. Remove any dependencies related to it."
            };
        }
        
        return await DefaultRemoveAsync<Service, ServiceDto>(_db, id);
    }
}