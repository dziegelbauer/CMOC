using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMOC.Pages.Status.Services;

public class Index : PageModel
{
    private readonly IObjectManager _objectManager;
    
    [BindProperty]
    public List<ServiceDto> Services { get; set; }
    
    [BindProperty]
    public string? CapabilityName { get; set; }
    
    public Index(IObjectManager objectManager)
    {
        _objectManager = objectManager;
        Services = new List<ServiceDto>();
    }
    
    public async Task OnGet(int? capabilityId)
    {
        if (capabilityId is not null)
        {
            CapabilityName = (await _objectManager.GetCapabilityAsync(c => c.Id == capabilityId))?.Name;
            Services = await _objectManager
                .GetServicesAsync(s => s.Supports.Any(csr => csr.CapabilityId == capabilityId));
        }
        else
        {
            CapabilityName = null;
            Services = await _objectManager.GetServicesAsync();
        }
    }
}