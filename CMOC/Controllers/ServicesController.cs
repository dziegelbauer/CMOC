using CMOC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public ServicesController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var serviceList = await _objectManager.GetServicesAsync();
        return Ok(new { data = serviceList });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveServiceAsync(id)
            ? Ok(new
            {
                success = true,
                message = $"Service with id: {id} deleted."
            })
            : NotFound();
    }
}