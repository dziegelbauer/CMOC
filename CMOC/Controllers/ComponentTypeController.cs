using System.Net.Mime;
using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ComponentTypeController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public ComponentTypeController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var componentTypeList = await _objectManager.GetComponentTypesAsync();
        return Ok(new { data = componentTypeList });
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var componentType = await _objectManager.GetComponentTypeAsync(et => et.Id == id);
        return componentType is not null
        ? Ok(new { data = componentType })
        : NotFound();
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Post([FromBody] ComponentTypeDto dto)
    {
        var componentType = await _objectManager.AddComponentTypeAsync(dto);
        return componentType.Id != 0
            ? StatusCode(StatusCodes.Status201Created, componentType)
            : BadRequest();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveComponentTypeAsync(id)
            ? Ok(new
            {
                success = true,
                message = $"Component type with id: {id} deleted."
            })
            : NotFound();
    }
}