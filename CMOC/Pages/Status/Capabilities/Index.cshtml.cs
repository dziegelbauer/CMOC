using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMOC.Pages.Status.Capabilities;

public class Index : PageModel
{
    private readonly IObjectManager _objectManager;
    
    [BindProperty]
    public List<CapabilityDto> Capabilities { get; set; }
    
    public Index(IObjectManager objectManager)
    {
        _objectManager = objectManager;
        Capabilities = new List<CapabilityDto>();
    }
    
    public async Task OnGet()
    {
        Capabilities = await _objectManager.GetCapabilitiesAsync();
    }
}