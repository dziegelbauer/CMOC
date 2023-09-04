using CMOC.Data;
using CMOC.Domain;
using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Pages.Admin.Capabilities;

public class Upsert : PageModel
{
    private readonly IObjectManager _objectManager;

    public Upsert(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [BindProperty]
    public CapabilityDto Capability { get; set; }
    
    public async Task OnGet(int? id)
    {
        if (id is not null)
        {
            Capability = await _objectManager
                .GetCapabilityAsync(c => c.Id == id) ?? new CapabilityDto();
        }
        else
        {
            Capability = new CapabilityDto();
        }
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (Capability.Id == 0) // Create
        {
            await _objectManager.AddCapabilityAsync(Capability);
        }
        else // Update
        {
            await _objectManager.UpdateCapabilityAsync(Capability);
        }
        
        return RedirectToPage("/Admin/Capabilities/Index");
    }
}