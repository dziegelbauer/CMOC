using System.Net.Mime;
using CMOC.Services;
using CMOC.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class IssuesController : ControllerBase
{
    private readonly IObjectManager _objectManager;

    public IssuesController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
    
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var issuesList = await _objectManager.GetIssuesAsync();
        return Ok(issuesList);
    }

    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var issue = await _objectManager.GetIssueAsync(i => i.Id == id);
        return issue.Result switch
        {
            ServiceResult.Success => Ok(issue),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] IssueDto dto)
    {
        if (dto.Id != 0) return BadRequest();
        var issue = await _objectManager.AddIssueAsync(dto);
        return Ok(issue);
    }
    
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromBody] IssueDto dto)
    {
        if (dto.Id == 0) return BadRequest();
        var issue = await _objectManager.UpdateIssueAsync(dto);
        return issue.Result switch
        {
            ServiceResult.Success => Ok(issue),
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
        var response = await _objectManager.RemoveIssueAsync(id);
        return response.Result switch
        {
            ServiceResult.Success => Ok(new
            {
                success = true,
                message = "Delete successful"
            }),
            ServiceResult.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}