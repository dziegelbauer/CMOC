using System.Linq.Expressions;

namespace CMOC.Services.Repository;

public interface IAssetRepository<T, TDto, TType, TTypeDto> : IRepository<T, TDto>  
    where T : class
    where TDto : class?
    where TType : class
    where TTypeDto : class?
{
    Task<ServiceResponse<TTypeDto>> GetTypeAsync(Expression<Func<TType, bool>>? filter = null);
    Task<ServiceResponse<List<TTypeDto>>> GetTypesAsync(Expression<Func<TType, bool>>? filter = null);
    Task<ServiceResponse<TTypeDto>> AddTypeAsync(TTypeDto dto);
    Task<ServiceResponse<TTypeDto>> UpdateTypeAsync(TTypeDto dto);
    Task<ServiceResponse<TTypeDto>> RemoveTypeAsync(int id);
}