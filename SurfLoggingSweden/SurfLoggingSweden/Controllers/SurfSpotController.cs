using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfLoggingSweden.Data;
using SurfLoggingSweden.Shared.Entities;

namespace SurfLoggingSweden.Controllers
{
    [Route("api/surfspots")]
    [ApiController]
    public class SurfSpotController : ControllerBase
    {
        private readonly DataContext _context;

        public SurfSpotController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SurfSpot>>> GetSurfSpotsAsync()
        {
            return await _context.SurfSpots.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<SurfSpot>> PostSurfSpot(SurfSpot surfSpot)
        {
            _context.SurfSpots.Add(surfSpot);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSurfSpotsAsync), new { id = surfSpot.Id }, surfSpot);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurfSpot(int id, SurfSpot surfSpot)
        {
            if (id != surfSpot.Id)
            {
                return BadRequest();
            }

            _context.Entry(surfSpot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurfSpotExists(id))
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

        private bool SurfSpotExists(int id)
        {
            return _context.SurfSpots.Any(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurfSpot(int id)
        {
            var surfSpot = await _context.SurfSpots.FindAsync(id);
            if (surfSpot == null)
            {
                return NotFound();
            }

            _context.SurfSpots.Remove(surfSpot);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
