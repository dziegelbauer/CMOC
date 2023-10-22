using CMOC.Data;

namespace CMOC.Services.Repository;

public abstract class Repository
{
    protected readonly AppDbContext _db;

    protected Repository(AppDbContext db)
    {
        _db = db;
    }

    protected static async Task<ServiceResponse<TDto>> DefaultRemoveAsync<T, TDto>(AppDbContext db, int id)
        where T : class
        where TDto : class
    {
        var objFromDb = await db.Set<T>().FindAsync(id);

        if (objFromDb is null)
        {
            return new ServiceResponse<TDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Object not found."
            };
        }

        db.Remove(objFromDb);
        await db.SaveChangesAsync();

        return new ServiceResponse<TDto>
        {
            Result = ServiceResult.Success,
            Payload = null,
            Message = "Object deleted successfully."
        };            
    }
}