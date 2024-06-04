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
    }
}
