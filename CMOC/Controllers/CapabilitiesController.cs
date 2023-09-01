using CMOC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CapabilitiesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CapabilitiesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var capabilityList = await _db.Capabilities.ToListAsync();
        return Ok(new { data = capabilityList });
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var objFromDb = await _db.Capabilities.FirstOrDefaultAsync(c => c.Id == id);

        if (objFromDb is not null)
        {
            _db.Remove(objFromDb);
            await _db.SaveChangesAsync();
        }
        
        return Ok(new
        {
            success = true,
            message = "Delete successful"
        });
    }
}