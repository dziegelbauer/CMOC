using System.Net.Mime;
using CMOC.Services;
using CMOC.Services.Dto;
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
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var componentsList = await _objectManager.GetComponentsAsync();
        return Ok(componentsList);
    }
    
    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id == 0) return Ok();
        var component = await _objectManager.GetComponentAsync(c => c.Id == id);
        return component.Result switch
        {
            ServiceResult.Success => Ok(component),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] ComponentDto dto)
    {
        if (dto.Id != 0) return BadRequest();
        var component = await _objectManager.AddComponentAsync(dto);
        return Ok(component);
    }
    
    [HttpPost("{id:int}/Issue/{issueId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostIssue(int id, int issueId)
    {
        var component = await _objectManager.AssignIssueToComponent(id, issueId);

        return component.Result switch
        { 
            ServiceResult.Success => Ok(component),
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
    public async Task<IActionResult> Put([FromBody] ComponentDto dto)
    {
        if (dto.Id == 0) return BadRequest();
        var component = await _objectManager.UpdateComponentAsync(dto);
        return component.Result switch
        {
            ServiceResult.Success => Ok(component),
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
        var response = await _objectManager.RemoveCapabilityAsync(id);
        return response.Result switch
        {
            ServiceResult.Success => Ok(new
            {
                success = true,
                message = $"Component with id: {id} deleted."
            }),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}