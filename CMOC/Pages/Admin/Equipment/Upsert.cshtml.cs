using CMOC.Services;
using CMOC.Services.Dto;
using CMOC.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMOC.Pages.Admin.Equipment;

public class Upsert : PageModel
{
    private readonly IObjectManager _objectManager;

    public Upsert(IObjectManager objectManager)
    {
        _objectManager = objectManager;
        EquipmentTypes = new List<SelectListItem>();
        Equipment = new EquipmentDto();
        Locations = new List<SelectListItem>();
    }

    [BindProperty]
    public EquipmentDto Equipment { get; set; }
    [BindProperty] 
    public List<SelectListItem> EquipmentTypes { get; set; }
    [BindProperty]
    public List<SelectListItem> Locations { get; set; }
    [BindProperty]
    public List<CheckedListRow> SupportedServices { get; set; }
    
    public async Task OnGet(int? id)
    {
        SupportedServices = (await _objectManager.GetServicesAsync())
            .Select(c => new CheckedListRow
            {
                Text = c.Name,
                Value = c.Id,
                Checked = false
            }).ToList();
        
        if (id is not null)
        {
            Equipment = await _objectManager
                .GetEquipmentItemAsync(e => e.Id == id) ?? new EquipmentDto();
        }
        else
        {
            Equipment = new EquipmentDto();
        }

        EquipmentTypes = (await _objectManager.GetEquipmentTypesAsync()).Select(et => new SelectListItem
        {
            Value = et.Id.ToString(),
            Text = et.Name
        }).ToList();

        Locations = (await _objectManager.GetLocationsAsync()).Select(l => new SelectListItem
        {
            Value = l.Id.ToString(),
            Text = l.Name
        }).ToList();
    }
    
    public async Task<IActionResult> OnPost()
    {
        Equipment.SupportedServices.Clear();
        
        foreach (var service in SupportedServices)
        {
            if (service.Checked)
            {
                Equipment.SupportedServices.Add(service.Value);
            }
        }
        
        if (Equipment.Id == 0) // Create
        {
            await _objectManager.AddEquipmentItemAsync(Equipment);
        }
        else // Update
        {
            await _objectManager.UpdateEquipmentItemAsync(Equipment);
        }
        
        return RedirectToPage("/Admin/Equipment/Index");
    }
}