using System.Linq.Expressions;
using CMOC.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public abstract class Repository<T, TDto> : IRepository<T, TDto> 
    where T : class
    where TDto : class
{
    protected readonly AppDbContext _db;

    protected Repository(AppDbContext db)
    {
        _db = db;
    }

    public virtual async Task<TDto?> GetAsync(Expression<Func<T, bool>>? filter = null)
    {
        return await DefaultGetAsync<T, TDto>(_db, filter);
    }

    public virtual async Task<List<TDto>> GetManyAsync(Expression<Func<T, bool>>? filter = null)
    {
        return await DefaultGetManyAsync<T, TDto>(_db, filter);
    }
    
    public virtual async Task<TDto> AddAsync(TDto dto)
    {
        return await DefaultAddAsync<T, TDto>(_db, dto);
    }

    public virtual async Task<TDto> UpdateAsync(TDto dto)
    {
        return await DefaultUpdateAsync<T, TDto>(_db, dto);
    }
    
    public virtual async Task<bool> RemoveAsync(int id)
    {
        return await DefaultRemoveAsync<T>(_db, id);
    }

    protected static async Task<TUDto?> DefaultGetAsync<TU, TUDto>(AppDbContext db, Expression<Func<TU, bool>>? filter = null) 
        where TU : class
        where TUDto : class?
    {
        var query = db.Set<TU>().AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<TUDto>();
    }

    protected static async Task<List<TUDto>> DefaultGetManyAsync<TU, TUDto>(AppDbContext db, Expression<Func<TU, bool>>? filter = null)
        where TU : class
        where TUDto : class
    {
        var query = db.Set<TU>().AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(t => t.Adapt<TUDto>())
            .ToList();
    }

    protected static async Task<TUDto> DefaultAddAsync<TU, TUDto>(AppDbContext db, TUDto dto)
        where TU : class
        where TUDto : class
    {
        var issue = await db.Set<TU>().AddAsync(dto.Adapt<TU>());
        await db.SaveChangesAsync();
        return issue.Entity.Adapt<TUDto>();
    }
    
    protected static async Task<TUDto> DefaultUpdateAsync<TU, TUDto>(AppDbContext db, TUDto dto)
        where TU : class
        where TUDto : class
    {
        var issue = db.Set<T>().Update(dto.Adapt<T>());
        await db.SaveChangesAsync();
        return issue.Entity.Adapt<TUDto>();
    }

    protected static async Task<bool> DefaultRemoveAsync<TU>(AppDbContext db, int id)
        where TU : class
    {
        var objFromDb = await db.Set<TU>().FindAsync(id);

        if (objFromDb is null)
        {
            return false;
        }

        db.Remove(objFromDb);
        await db.SaveChangesAsync();

        return true;            
    }
}