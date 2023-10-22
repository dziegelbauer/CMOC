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
    
    public async Task<ServiceResponse<IssueDto>> GetAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        var query = _db.Issues.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return new ServiceResponse<IssueDto>
            {
                Result = ServiceResult.NotFound,
                Payload = null,
                Message = "Issue not found."
            };
        }

        return new ServiceResponse<IssueDto>
        {
            Result = ServiceResult.Success,
            Payload = queryResult.ToDto(),
            Message = "Successfully returned issue."
        };
    }
    
    public async Task<ServiceResponse<List<IssueDto>>> GetManyAsync(Expression<Func<Issue, bool>>? filter = null)
    {
        var query = _db.Issues.AsNoTracking().AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var queryResult = await query.ToListAsync();

        return new ServiceResponse<List<IssueDto>>
        {
            Result = ServiceResult.Success,
            Payload = queryResult
                .Select(t => t.ToDto())
                .ToList(),
            Message = "Successfully returned issues."
        };
    }

    public async Task<ServiceResponse<IssueDto>> AddAsync(IssueDto dto)
    {
        var entity = await _db.Issues.AddAsync(dto.ToEntity());
        await _db.SaveChangesAsync();
        
        var idProperty = entity.Entity.Id;
        
        return await GetAsync(i => i.Id == idProperty);
    }

    public async Task<ServiceResponse<IssueDto>> UpdateAsync(IssueDto dto)
    {
        var entity = _db.Issues.Update(dto.ToEntity());
        await _db.SaveChangesAsync();
        return await GetAsync(i => i.Id == entity.Entity.Id);
    }

    public async Task<ServiceResponse<IssueDto>> RemoveAsync(int id)
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

        return await DefaultRemoveAsync<Issue, IssueDto>(_db, id);
    }
}