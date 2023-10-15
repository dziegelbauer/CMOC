using System.Linq.Expressions;
using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using CMOC.Services.Utility;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class IssueRepository : Repository, IIssueRepository
{
    public IssueRepository(AppDbContext db) : base(db)
    {
    }
    
    public async Task<IssueDto?> GetAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        var query = _db.Issues.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        return queryResult?.ToDto();
    }
    
    public async Task<List<IssueDto>> GetManyAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        var query = _db.Issues.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return queryResult
            .Select(t => t.ToDto())
            .ToList();
    }
    
    private async Task<IssueDto?> GetByIdAsync(int id) 
    {
        var queryResult = await _db.Issues.FindAsync(id);

        return queryResult?.ToDto();
    }

    public async Task<IssueDto> AddAsync(IssueDto dto)
    {
        var entity = await _db.Issues.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        
        var idProperty = entity.Entity.Id;
        
        return await GetByIdAsync(idProperty) ?? throw new Exception();
    }

    public async Task<IssueDto> UpdateAsync(IssueDto dto)
    {
        var entity = _db.Issues.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetByIdAsync(entity.Entity.Id) ?? throw new Exception();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        await _db.Equipment
            .Where(e => e.IssueId == id)
            .ForEachAsync(e =>
            {
                e.IssueId = null;
                _db.Equipment.Update(e);
            });
        await _db.Components
            .Where(c => c.IssueId == id)
            .ForEachAsync(c =>
            {
                c.IssueId = null;
                _db.Components.Update(c);
            });

        await _db.SaveChangesAsync();

        return await DefaultRemoveAsync<Issue>(_db, id);
    }
}