using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Repository;

public interface IServiceRepository : IRepository<Service, ServiceDto>
{
    
}