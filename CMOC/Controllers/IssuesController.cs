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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var issue = await _objectManager.GetIssueAsync(i => i.Id == id);
        return Ok(issue);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] IssueDto dto)
    {
        var issue = await _objectManager.AddIssueAsync(dto);
        return Ok(issue);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _objectManager.RemoveIssueAsync(id)
            ? Ok(new
            {
                success = true,
                message = "Delete successful"
            })
            : NotFound();
    }
}