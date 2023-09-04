using System.Linq.Expressions;

namespace CMOC.Services.Repository;

public interface IAssetRepository<T, TDto, TType, TTypeDto> : IRepository<T, TDto>  
    where T : class
    where TDto : class?
    where TType : class
    where TTypeDto : class?
{
    Task<TTypeDto?> GetTypeAsync(Expression<Func<TType, bool>>? filter = null);
    Task<List<TTypeDto>> GetTypesAsync(Expression<Func<TType, bool>>? filter = null);
    Task<TTypeDto> AddTypeAsync(TTypeDto dto);
    Task<TTypeDto> UpdateTypeAsync(TTypeDto dto);
    Task<bool> RemoveTypeAsync(int id);
}