using CMOC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("ap1/v1/[controller]")]
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
        var equipmentList = await _objectManager.GetEquipmentItemsAsync();
        return Ok(new { data = equipmentList });
    }
    
    [HttpPost("{id:int}/Issue/{issueId:int}")]
    public async Task<IActionResult> PostIssue(int id, int issueId)
    {
        var component = await _objectManager.AssignIssueToComponent(id, issueId);

        return component is not null
            ? Ok(component)
            : NotFound();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveEquipmentItemAsync(id)
            ? Ok(new
            {
                success = true,
                message = $"Component with id: {id} deleted."
            })
            : NotFound();
    }
}