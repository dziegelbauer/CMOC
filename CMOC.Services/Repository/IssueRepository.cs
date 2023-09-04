using CMOC.Data;
using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Repository;

public class IssueRepository : Repository<Issue, IssueDto>, IIssueRepository
{
    public IssueRepository(AppDbContext db) : base(db)
    {
    }

    public override async Task<bool> RemoveAsync(int id)
    {
        var issueInUse = _db.Equipment.Any(e => e.LocationId == id) 
                         || _db.Components.Any(c => c.IssueId == id);

        if (issueInUse)
        {
            return false;
        }

        return await DefaultRemoveAsync<Issue>(_db, id);
    }
}