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

        var issue = await _db.Issues.FindAsync(id);

        if (issue is null)
        {
            return false;
        }

        _db.Issues.Remove(issue);
        await _db.SaveChangesAsync();
        return true;
    }
}