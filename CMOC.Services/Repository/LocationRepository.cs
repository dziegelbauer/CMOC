using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Utility;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class LocationRepository : Repository, ILocationRepository
{
    public LocationRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<ServiceResponse<LocationDto>> GetAsync(Expression<Func<Location, bool>>? filter = null)
    {
        var query = _db.Locations.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return new ServiceResponse<LocationDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Location not found."
            };
        }

        return new ServiceResponse<LocationDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned location."
        };
    }

    public async Task<ServiceResponse<List<LocationDto>>> GetManyAsync(Expression<Func<Location, bool>>? filter = null)
    {
        var query = _db.Locations.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return new ServiceResponse<List<LocationDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(l => l.ToDto())
                .ToList(),
            Message = "Successfully returned locations."
        };
    }

    public async Task<ServiceResponse<LocationDto>> AddAsync(LocationDto dto)
    {
        var newLocation = await _db.Locations.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(l => l.Id == newLocation.Entity.Id);
    }

    public async Task<ServiceResponse<LocationDto>> UpdateAsync(LocationDto dto)
    {
        var updatedLocation = _db.Locations.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(l => l.Id == updatedLocation.Entity.Id);
    }

    public async Task<ServiceResponse<LocationDto>> RemoveAsync(int id)
    {
        var locationInUse = _db.Equipment.Any(e => e.LocationId == id);

        if (locationInUse)
        {
            return new ServiceResponse<LocationDto>
            {
                Result = ServiceResult.InUse,
                Payload = null,
                Message = "Location not found."
            };
        }

        return await DefaultRemoveAsync<Location, LocationDto>(_db, id);
    }
}