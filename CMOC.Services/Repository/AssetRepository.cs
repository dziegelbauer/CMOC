using System.Linq.Expressions;
using CMOC.Data;

namespace CMOC.Services.Repository;

public abstract class AssetRepository<T, TDto, TType, TTypeDto> : Repository<T, TDto>, IAssetRepository<T, TDto, TType, TTypeDto> 
    where T : class 
    where TDto : class
    where TType : class
    where TTypeDto : class
{
    protected AssetRepository(AppDbContext db) : base(db)
    {
    }

    public virtual async Task<TTypeDto?> GetTypeAsync(Expression<Func<TType, bool>>? filter = null)
    {
        return await DefaultGetAsync<TType, TTypeDto>(_db, filter);
    }

    public async Task<List<TTypeDto>> GetTypesAsync(Expression<Func<TType, bool>>? filter = null)
    {
        return await DefaultGetManyAsync<TType, TTypeDto>(_db, filter);
    }

    public async Task<TTypeDto> AddTypeAsync(TTypeDto dto)
    {
        return await DefaultAddAsync<TType, TTypeDto>(_db, dto);
    }

    public async Task<TTypeDto> UpdateTypeAsync(TTypeDto dto)
    {
        return await DefaultUpdateAsync<TType, TTypeDto>(_db, dto);
    }

    public async Task<bool> RemoveTypeAsync(int id)
    {
        return await DefaultRemoveAsync<TType>(_db, id);
    }
}