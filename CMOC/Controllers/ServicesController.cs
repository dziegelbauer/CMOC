using System.Net.Mime;
using CMOC.Services;
using CMOC.Services.Dto;
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
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var serviceList = await _objectManager.GetServicesAsync();
        return Ok(serviceList);
    }
    
    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id == 0) return Ok();
        var service = await _objectManager.GetServiceAsync(s => s.Id == id);
        return service.Result switch
        {
            ServiceResult.Success => Ok(service),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
    
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] ServiceDto dto)
    {
        if (dto.Id != 0) return BadRequest();
        var service = await _objectManager.AddServiceAsync(dto);
        return Ok(service);
    }

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromBody] ServiceDto dto)
    {
        if (dto.Id == 0) return BadRequest();
        var service = await _objectManager.UpdateServiceAsync(dto);
        return service.Result switch
        {
            ServiceResult.Success => Ok(service),
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
        var response = await _objectManager.RemoveServiceAsync(id);
        return response.Result switch
        {
            ServiceResult.Success => Ok(new
            {
                success = true,
                message = $"Service with id: {id} deleted."
            }),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}