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
    public async Task<IActionResult> Get()
    {
        var equipmentTypeList = await _objectManager.GetEquipmentTypesAsync();
        return Ok(new { data = equipmentTypeList });
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var equipmentType = await _objectManager.GetEquipmentTypeAsync(et => et.Id == id);
        return equipmentType is not null
        ? Ok(new { data = equipmentType })
        : NotFound();
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Post([FromBody] EquipmentTypeDto dto)
    {
        var equipmentType = await _objectManager.AddEquipmentTypeAsync(dto);
        return equipmentType.Id != 0
            ? StatusCode(StatusCodes.Status201Created, equipmentType)
            : BadRequest();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveEquipmentTypeAsync(id)
            ? NoContent()
            : NotFound();
    }
}