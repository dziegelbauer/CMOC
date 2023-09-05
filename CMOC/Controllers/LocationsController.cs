using CMOC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public LocationsController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var locationsList = await _objectManager.GetLocationsAsync();
        return Ok(new { data = locationsList });
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveLocationAsync(id)
            ? Ok(new
            {
                success = true,
                message = "Delete successful"
            })
            : NotFound();
    }
}