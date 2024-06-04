using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfLoggingSweden.Data;
using SurfLoggingSweden.Shared.Entities;

namespace SurfLoggingSweden.Controllers;

[Route("api/surfsessions")]
[ApiController]
public class SurfSessionController : ControllerBase
{
    private readonly DataContext _context;

    public SurfSessionController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<SurfSession>>> GetSurfSessionsAsync()
    {
        return await _context.SurfSessions.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<SurfSession>> PostSurfSession(SurfSession surfSession)
    {
        _context.SurfSessions.Add(surfSession);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSurfSessionsAsync), new { id = surfSession.Id }, surfSession);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSurfSession(int id, SurfSession surfSession)
    {
        if (id != surfSession.Id)
        {
            return BadRequest();
        }

        _context.Entry(surfSession).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SurfSessionExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSurfSession(int id)
    {
        var surfSession = await _context.SurfSessions.FindAsync(id);
        if (surfSession == null)
        {
            return NotFound();
        }

        _context.SurfSessions.Remove(surfSession);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SurfSessionExists(int id)
    {
        return _context.SurfSessions.Any(e => e.Id == id);
    }

}