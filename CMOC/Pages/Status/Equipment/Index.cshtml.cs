using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMOC.Pages.Status.Equipment;

public class Index : PageModel
{
    private readonly IObjectManager _objectManager;
    
    [BindProperty]
    public List<EquipmentDto> Equipment { get; set; }
    
    [BindProperty]
    public string? ServiceName { get; set; }
    
    public Index(IObjectManager objectManager)
    {
        _objectManager = objectManager;
        Equipment = new List<EquipmentDto>();
    }
    
    public async Task OnGet(int? serviceId)
    {
        if (serviceId is not null)
        {
            ServiceName = (await _objectManager.GetCapabilityAsync(c => c.Id == serviceId))?.Name;
            Equipment = await _objectManager
                .GetEquipmentItemsAsync(e => e.Relationships.Any(ssr => ssr.ServiceId == serviceId));
        }
        else
        {
            ServiceName = null;
            Equipment = await _objectManager.GetEquipmentItemsAsync();
        }
    }
}