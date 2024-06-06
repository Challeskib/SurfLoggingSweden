using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfLoggingSweden.Data;
using SurfLoggingSweden.Shared.Entities;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using SurfLoggingSweden.Shared.Entities;
using SurfLoggingSweden.Shared.Models;

namespace SurfLoggingSweden.Controllers
{
    [Route("api/surfspots")]
    [ApiController]
    public class SurfSpotController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;

        public SurfSpotController(DataContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet("with-condition")]
        public async Task<ActionResult<List<SurfSpotWithCondition>>> GetSurfSpotsWithCondition()
        {
            var surfSpots = await _context.SurfSpots.ToListAsync();
            var tasks = surfSpots.Select(async spot =>
            {
                var condition = await FetchWeatherCondition(spot.Location);
                return new SurfSpotWithCondition
                {
                    Id = spot.Id,
                    Name = spot.Name,
                    Location = spot.Location,
                    WindDegree = condition?.current?.wind_degree ?? 0,
                    WindSpeedMps = condition?.current?.wind_kph / 3.6 ?? 0 // Convert kph to mps
                };
            });
            var surfSpotsWithCondition = await Task.WhenAll(tasks);
            return Ok(surfSpotsWithCondition);
        }

        private async Task<WeatherCondition> FetchWeatherCondition(string location)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"https://api.weatherapi.com/v1/current.json?q={location}&key=c128a303ed08474c96890114232709");
                return JsonSerializer.Deserialize<WeatherCondition>(response);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<SurfSpot>>> GetSurfSpotsAsync()
        {
            return await _context.SurfSpots.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SurfSpot>> GetSurfSpot(int id)
        {
            var surfSpot = await _context.SurfSpots.FindAsync(id);

            if (surfSpot == null)
            {
                return NotFound();
            }

            return surfSpot;
        }

        [HttpPost]
        public async Task<ActionResult<SurfSpot>> PostSurfSpot(SurfSpot surfSpot)
        {
            _context.SurfSpots.Add(surfSpot);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSurfSpot), new { id = surfSpot.Id }, surfSpot);
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

        private bool SurfSpotExists(int id)
        {
            return _context.SurfSpots.Any(e => e.Id == id);
        }

        public class WeatherCondition
        {
            public Current current { get; set; }

            public class Current
            {
                public int wind_degree { get; set; }
                public double wind_kph { get; set; }
            }
        }
    }
}
