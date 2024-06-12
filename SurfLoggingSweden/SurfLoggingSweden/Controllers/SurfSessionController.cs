using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfLoggingSweden.Data;
using SurfLoggingSweden.Shared.Models;
using SurfLoggingSweden.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLoggingSweden.Controllers
{
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
            var result = await _context.SurfSessions
                .ToListAsync();

            return result;
        }

        [HttpGet("surfsessionsdto")]
        public async Task<ActionResult<List<SurfSessionDto>>> GetSurfSessionsDtoAsync()
        {
            var sessions = await _context.SurfSessions
                .Include(s => s.SurfSpot)
                .ToListAsync();

            var sessionDtos = sessions.Select(s => new SurfSessionDto
            {
                Id = s.Id,
                SurfSpotId = s.SurfSpotId,
                SurfSpotName = s.SurfSpot?.Name,
                WindDegree = s.WindDegree,
                Rating = s.Rating,
                WindPower = s.WindPower,
                Start = s.Start,
                End = s.End
            }).ToList();

            return Ok(sessionDtos);
        }



        [HttpGet("spotsessioncounts")]
        public async Task<ActionResult<List<SurfSpotSessionCount>>> GetSurfSpotSessionCounts()
        {
            try
            {
                var result = await _context.SurfSessions
                    .Include(s => s.SurfSpot)
                    .GroupBy(s => s.SurfSpot.Name)
                    .Select(g => new SurfSpotSessionCount
                    {
                        SurfSpotName = g.Key,
                        SessionCount = g.Count()
                    })
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
}
