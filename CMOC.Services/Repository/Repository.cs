using CMOC.Data;

namespace CMOC.Services.Repository;

public abstract class Repository
{
    protected readonly AppDbContext _db;

    protected Repository(AppDbContext db)
    {
        _db = db;
    }

    protected static async Task<bool> DefaultRemoveAsync<T>(AppDbContext db, int id)
        where T : class
    {
        var objFromDb = await db.Set<T>().FindAsync(id);

        if (objFromDb is null)
        {
            return false;
        }

        db.Remove(objFromDb);
        await db.SaveChangesAsync();

        return true;            
    }
}