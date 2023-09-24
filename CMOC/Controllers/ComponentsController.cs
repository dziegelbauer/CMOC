using CMOC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ComponentsController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public ComponentsController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var componentsList = await _objectManager.GetComponentsAsync();
        return Ok(new { data = componentsList });
    }
    
    [HttpPost("{id:int}/Issue/{issueId:int}")]
    public async Task<IActionResult> PostIssue(int id, int issueId)
    {
        var component = await _objectManager.AssignIssueToComponent(id, issueId);

        return component is not null
            ? Ok(component)
            : NotFound();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveComponentAsync(id)
            ? Ok(new
            {
                success = true,
                message = $"Component with id: {id} deleted."
            })
            : NotFound();
    }
}