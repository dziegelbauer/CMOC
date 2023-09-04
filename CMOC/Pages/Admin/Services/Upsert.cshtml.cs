using CMOC.Services;
using CMOC.Services.Dto;
using CMOC.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMOC.Pages.Admin.Services;

public class Upsert : PageModel
{
    private readonly IObjectManager _objectManager;

    public Upsert(IObjectManager objectManager)
    {
        _objectManager = objectManager;
        Service = new ServiceDto();
        SupportedCapabilities = new List<CheckedListRow>();
    }

    [BindProperty]
    public ServiceDto Service { get; set; }

    [BindProperty]
    public List<CheckedListRow> SupportedCapabilities { get; set; }
    
    public async Task OnGet(int? id)
    {
        SupportedCapabilities = (await _objectManager.GetCapabilitiesAsync())
            .Select(c => new CheckedListRow
        {
            Text = c.Name,
            Value = c.Id,
            Checked = false
        }).ToList();
        
        if (id is not null)
        {
            Service = await _objectManager.GetServiceAsync(s => s.Id == id) ?? new ServiceDto();
        }
        else
        {
            Service = new ServiceDto();
        }

        foreach (var relationship in Service.Dependents)
        {
            var checkRow = SupportedCapabilities.FirstOrDefault(sc => sc.Value == relationship);

            if (checkRow is not null)
            {
                checkRow.Checked = true;
            }
        }
    }
    
    public async Task<IActionResult> OnPost()
    {
        Service.Dependents.Clear();
        
        foreach (var capability in SupportedCapabilities)
        {
            if (capability.Checked)
            {
                Service.Dependents.Add(capability.Value);
            }
        }
        
        if (Service.Id == 0) // Create
        {
            await _objectManager.AddServiceAsync(Service);
        }
        else // Update
        {
            await _objectManager.UpdateServiceAsync(Service);
        }
        
        return RedirectToPage("/Admin/Services/Index");
    }
}