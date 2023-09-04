using CMOC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public LocationsController(AppDbContext db)
    {
        _db = db;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var locationsList = await _db.Locations.ToListAsync();
        return Ok(new { data = locationsList });
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var objFromDb = await _db.Locations.FirstOrDefaultAsync(c => c.Id == id);

        if (objFromDb is null)
        {
            return NotFound();
        }
        
        _db.Remove(objFromDb);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Delete successful"
        });
    }
}