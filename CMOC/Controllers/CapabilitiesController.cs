using System.Net.Mime;
using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CapabilitiesController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public CapabilitiesController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var capabilityList = await _objectManager.GetCapabilitiesAsync();
        return Ok(capabilityList);
    }

    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id == 0) return Ok();
        var capability = await _objectManager.GetCapabilityAsync(c => c.Id == id);
        return capability.Result switch
        {
            ServiceResult.Success => Ok(capability),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CapabilityDto dto)
    {
        if (dto.Id != 0) return BadRequest();
        var capability = await _objectManager.AddCapabilityAsync(dto);
        return Ok(capability);
    }

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromBody] CapabilityDto dto)
    {
        if (dto.Id == 0) return BadRequest();
        var capability = await _objectManager.UpdateCapabilityAsync(dto);
        return capability.Result switch
        {
            ServiceResult.Success => Ok(capability),
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
                message = $"Capability with id: {id} deleted."
            }),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}