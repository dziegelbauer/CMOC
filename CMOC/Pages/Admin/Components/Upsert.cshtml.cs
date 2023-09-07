using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMOC.Pages.Admin.Components;

public class Upsert : PageModel
{
    private readonly IObjectManager _objectManager;

    public Upsert(IObjectManager objectManager)
    {
        _objectManager = objectManager;
        ComponentTypes = new List<SelectListItem>();
        Component = new ComponentDto();
        Equipment = new List<SelectListItem>();
    }

    [BindProperty]
    public ComponentDto Component { get; set; }
    [BindProperty] 
    public List<SelectListItem> ComponentTypes { get; set; }
    [BindProperty]
    public List<SelectListItem> Equipment { get; set; }
    
    public async Task OnGet(int? id)
    {
        if (id is not null)
        {
            Component = await _objectManager
                .GetComponentAsync(c => c.Id == id) ?? new ComponentDto();
        }
        else
        {
            Component = new ComponentDto();
        }

        ComponentTypes = (await _objectManager.GetComponentTypesAsync()).Select(ct => new SelectListItem
        {
            Value = ct.Id.ToString(),
            Text = ct.Name
        }).ToList();

        Equipment = (await _objectManager.GetEquipmentItemsAsync()).Select(e => new SelectListItem
        {
            Value = e.Id.ToString(),
            Text = $"{e.TypeName} ({e.SerialNumber})"
        }).ToList();
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (Component.Id == 0) // Create
        {
            await _objectManager.AddComponentAsync(Component);
        }
        else // Update
        {
            await _objectManager.UpdateComponentAsync(Component);
        }
        
        return RedirectToPage("/Admin/Components/Index");
    }
}