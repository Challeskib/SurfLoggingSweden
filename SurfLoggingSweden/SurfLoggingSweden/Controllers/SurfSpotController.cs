using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfLoggingSweden.Data;
using SurfLoggingSweden.Shared.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using SurfLoggingSweden.Shared.Entities;

namespace SurfLoggingSweden.Controllers
{
    [Route("api/surfspots")]
    [ApiController]
    public class SurfSpotController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _weatherApiKey;


        public SurfSpotController(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
            _weatherApiKey = configuration["WeatherApi:ApiKey"];
        }

        [HttpGet("with-condition")]
        public async Task<ActionResult<List<SurfSpotWithCondition>>> GetSurfSpotsWithCondition()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var surfSpots = await context.SurfSpots.ToListAsync();
                var tasks = surfSpots.Select(async spot =>
                {
                    var condition = await FetchWeatherCondition(spot.Location);
                    var isSurfable = await CalculateSurfability(spot.Id, condition.current.wind_degree, condition.current.wind_kph / 3.6);

                    return new SurfSpotWithCondition
                    {
                        Id = spot.Id,
                        Name = spot.Name,
                        Location = spot.Location,
                        WindDegree = condition?.current?.wind_degree ?? 0,
                        WindSpeedMps = condition?.current?.wind_kph / 3.6 ?? 0, // Convert kph to mps
                        Surfable = isSurfable
                    };
                });
                var surfSpotsWithCondition = await Task.WhenAll(tasks);
                return Ok(surfSpotsWithCondition);
            }
        }

        private async Task<WeatherCondition> FetchWeatherCondition(string location)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var url = $"https://api.weatherapi.com/v1/current.json?q={location}&key={_weatherApiKey}";
                var response = await httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<WeatherCondition>(response);
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> CalculateSurfability(int surfSpotId, int currentWindDegree, double currentWindSpeedMps)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var highRatingSessions = await context.SurfSessions
                    .Where(session => session.SurfSpotId == surfSpotId && session.Rating >= 3)
                    .ToListAsync();

                if (!highRatingSessions.Any())
                    return false;

                var matchingSessions = highRatingSessions
                    .Where(session =>
                        Math.Abs(session.WindDegree - currentWindDegree) <= 45 &&
                        Math.Abs(session.WindPower - currentWindSpeedMps) <= 3);

                return matchingSessions.Any();
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<SurfSpot>>> GetSurfSpotsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                return await context.SurfSpots.ToListAsync();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SurfSpot>> GetSurfSpot(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var surfSpot = await context.SurfSpots.FindAsync(id);

                if (surfSpot == null)
                {
                    return NotFound();
                }

                return surfSpot;
            }
        }

        [HttpPost]
        public async Task<ActionResult<SurfSpot>> PostSurfSpot(SurfSpot surfSpot)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                context.SurfSpots.Add(surfSpot);
                await context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSurfSpot), new { id = surfSpot.Id }, surfSpot);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurfSpot(int id, SurfSpot surfSpot)
        {
            if (id != surfSpot.Id)
            {
                return BadRequest();
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                context.Entry(surfSpot).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurfSpotExists(context, id))
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
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurfSpot(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var surfSpot = await context.SurfSpots.FindAsync(id);
                if (surfSpot == null)
                {
                    return NotFound();
                }

                context.SurfSpots.Remove(surfSpot);
                await context.SaveChangesAsync();

                return NoContent();
            }
        }

        private bool SurfSpotExists(DataContext context, int id)
        {
            return context.SurfSpots.Any(e => e.Id == id);
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
