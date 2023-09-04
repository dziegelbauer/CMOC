using System.Linq.Expressions;

namespace CMOC.Services.Repository;

public interface IRepository<T, TDto> where T : class where TDto : class?
{
    Task<TDto?> GetAsync(Expression<Func<T, bool>>? filter = null);
    Task<List<TDto>> GetManyAsync(Expression<Func<T, bool>>? filter = null);
    Task<TDto> AddAsync(TDto dto);
    Task<TDto> UpdateAsync(TDto dto);
    Task<bool> RemoveAsync(int id);
}