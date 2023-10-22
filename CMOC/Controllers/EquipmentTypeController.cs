using System.Net.Mime;
using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class EquipmentTypeController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public EquipmentTypeController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var equipmentTypeList = await _objectManager.GetEquipmentTypesAsync();
        return Ok(equipmentTypeList);
    }
    
    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var equipmentType = await _objectManager.GetEquipmentTypeAsync(et => et.Id == id);
        return equipmentType.Result switch
        {
            ServiceResult.Success => Ok(new { data = equipmentType }),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] EquipmentTypeDto dto)
    {
        var equipmentType = await _objectManager.AddEquipmentTypeAsync(dto);
        return equipmentType.Result switch
        {
            ServiceResult.Success => Ok(equipmentType),
            _ => BadRequest()
        };
    }
    
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromBody] EquipmentTypeDto dto)
    {
        if (dto.Id == 0) return BadRequest();
        var equipmentType = await _objectManager.UpdateEquipmentTypeAsync(dto);
        return equipmentType.Result switch
        {
            ServiceResult.Success => Ok(equipmentType),
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
        var response = await _objectManager.RemoveEquipmentTypeAsync(id);
        return response.Result switch
        {
            ServiceResult.Success => Ok(new
            {
                success = true,
                message = $"Equipment type with id: {id} deleted."
            }),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}