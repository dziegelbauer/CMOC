using System.Linq.Expressions;

namespace CMOC.Services.Repository;

public interface IRepository<T, TDto> where T : class where TDto : class?
{
    Task<ServiceResponse<TDto>> GetAsync(Expression<Func<T, bool>>? filter = null);
    Task<ServiceResponse<List<TDto>>> GetManyAsync(Expression<Func<T, bool>>? filter = null);
    Task<ServiceResponse<TDto>> AddAsync(TDto dto);
    Task<ServiceResponse<TDto>> UpdateAsync(TDto dto);
    Task<ServiceResponse<TDto>> RemoveAsync(int id);
}