using CMOC.Data;
using CMOC.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Pages.Admin.Capabilities;

public class Upsert : PageModel
{
    private readonly AppDbContext _db;

    public Upsert(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public Capability Capability { get; set; }
    
    public async Task OnGet(int? id)
    {
        if (id is not null)
        {
            Capability = await _db.Capabilities.FirstOrDefaultAsync(u => u.Id == id) ?? new Capability();
        }
        else
        {
            Capability = new Capability();
        }
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (Capability.Id == 0) // Create
        {
            _db.Capabilities.Add(Capability);
        }
        else // Update
        {
            _db.Update(Capability);
        }
        
        await _db.SaveChangesAsync();
        
        return RedirectToPage("/Admin/Capabilities/Index");
    }
}