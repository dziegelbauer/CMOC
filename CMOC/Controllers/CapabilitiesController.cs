using CMOC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CapabilitiesController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public CapabilitiesController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var capabilityList = await _objectManager.GetCapabilitiesAsync();
        return Ok(new { data = capabilityList });
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveCapabilityAsync(id)
            ? Ok(new
            {
                success = true,
                message = $"Capability with id: {id} deleted."
            })
            : NotFound();
    }
}