using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class ComponentRepository : AssetRepository<Component, ComponentDto, ComponentType, ComponentTypeDto>, IComponentRepository
{
    public ComponentRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<ComponentDto?> GetAsync(Expression<Func<Component, bool>>? filter = null)
    {
        var query = _db.Components.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.ComponentOf)
            .ThenInclude(cr => cr.Equipment);

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.Adapt<ComponentDto>();
    }

    public override async Task<List<ComponentDto>> GetManyAsync(Expression<Func<Component, bool>>? filter = null)
    {
        var query = _db.Components.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        query = query
            .Include(e => e.Type)
            .Include(e => e.Issue)
            .Include(e => e.ComponentOf)
            .ThenInclude(cr => cr.Equipment);

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(c => c.Adapt<ComponentDto>())
            .ToList();
    }
}