using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Services.Repository;

public class IssueRepository : Repository<Issue, IssueDto>, IIssueRepository
{
    public IssueRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<bool> RemoveAsync(int id)
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