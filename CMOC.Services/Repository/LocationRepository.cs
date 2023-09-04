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

        return await DefaultRemoveAsync<Location>(_db, id);
    }
}