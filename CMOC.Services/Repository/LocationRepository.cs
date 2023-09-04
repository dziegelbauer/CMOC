using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Repository;

public class LocationRepository : Repository<Location, LocationDto>, ILocationRepository
{
    public LocationRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<bool> RemoveAsync(int id)
    {
        var locationInUse = _db.Equipment.Any(e => e.LocationId == id);

        if (locationInUse)
        {
            return false;
        }

        var location = await _db.Locations.FindAsync(id);

        if (location is null)
        {
            return false;
        }

        _db.Locations.Remove(location);
        await _db.SaveChangesAsync();
        return true;
    }
}