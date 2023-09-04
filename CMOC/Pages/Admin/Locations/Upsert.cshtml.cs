using CMOC.Data;
using CMOC.Domain;
using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Pages.Admin.Locations;

public class Upsert : PageModel
{
    private readonly IObjectManager _objectManager;

    public Upsert(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [BindProperty]
    public LocationDto Location { get; set; }
    
    public async Task OnGet(int? id)
    {
        if (id is not null)
        {
            Location = await _objectManager
                .GetLocationAsync(l => l.Id == id) ?? new LocationDto();
        }
        else
        {
            Location = new LocationDto();
        }
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (Location.Id == 0) // Create
        {
            await _objectManager.AddLocationAsync(Location);
        }
        else // Update
        {
            await _objectManager.UpdateLocationAsync(Location);
        }
        
        return RedirectToPage("/Admin/Locations/Index");
    }
}