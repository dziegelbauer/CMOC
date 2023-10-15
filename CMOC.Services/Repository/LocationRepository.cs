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

    public async Task<LocationDto?> GetAsync(Expression<Func<Location, bool>>? filter = null)
    {
        var query = _db.Locations.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.ToDto();
    }

    public async Task<List<LocationDto>> GetManyAsync(Expression<Func<Location, bool>>? filter = null)
    {
        var query = _db.Locations.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(l => l.ToDto())
            .ToList();
    }

    public async Task<LocationDto> AddAsync(LocationDto dto)
    {
        var newLocation = await _db.Locations.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(l => l.Id == newLocation.Entity.Id) ?? throw new Exception();
    }

    public async Task<LocationDto> UpdateAsync(LocationDto dto)
    {
        var updatedLocation = _db.Locations.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(l => l.Id == updatedLocation.Entity.Id) ?? throw new Exception();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var locationInUse = _db.Equipment.Any(e => e.LocationId == id);

        if (locationInUse)
        {
            return false;
        }

        return await DefaultRemoveAsync<Location>(_db, id);
    }
}