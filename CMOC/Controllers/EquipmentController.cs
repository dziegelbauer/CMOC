using System.Net.Mime;
using CMOC.Services;
using CMOC.Services.Dto;
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
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var equipmentList = await _objectManager.GetEquipmentItemsAsync();
        return Ok(equipmentList);
    }
    
    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id == 0) return Ok();
        var equipment = await _objectManager.GetEquipmentItemsAsync(e => e.Id == id);
        return equipment.Result switch
        {
            ServiceResult.Success => Ok(equipment),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] EquipmentDto dto)
    {
        if (dto.Id != 0) return BadRequest();
        var equipment = await _objectManager.AddEquipmentItemAsync(dto);
        return Ok(equipment);
    }

    [HttpPost("{id:int}/Issue/{issueId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostIssue(int id, int issueId)
    {
        var equipment = await _objectManager.AssignIssueToEquipment(id, issueId);
        
        return equipment.Result switch
        { 
            ServiceResult.Success => Ok(equipment),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
    
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromBody] EquipmentDto dto)
    {
        if (dto.Id == 0) return BadRequest();
        var equipment = await _objectManager.UpdateEquipmentItemAsync(dto);
        return equipment.Result switch
        {
            ServiceResult.Success => Ok(equipment),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
    
    [HttpDelete("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _objectManager.RemoveEquipmentItemAsync(id);
        return response.Result switch
        {
            ServiceResult.Success => Ok(new
            {
                success = true,
                message = $"Equipment with id: {id} deleted."
            }),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}