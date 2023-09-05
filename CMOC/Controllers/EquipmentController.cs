using CMOC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class EquipmentController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public EquipmentController(IObjectManager objectManager)
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
        var equipment = await _objectManager.AssignIssueToEquipment(id, issueId);

        return equipment is not null
            ? Ok(equipment)
            : NotFound();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveEquipmentItemAsync(id)
            ? Ok(new
            {
                success = true,
                message = $"Equipment with id: {id} deleted."
            })
            : NotFound();
    }
}