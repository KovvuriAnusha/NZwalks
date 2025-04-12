using Microsoft.AspNetCore.Mvc;
using System;

namespace VersionAPI.v1.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("api/v1/WeatherForecast")]
        [ApiExplorerSettings(GroupName = "v1")]
        public IActionResult GetV1()
        {
            var data = Enumerable.Range(1, 5).Select(i => new WeatherForecastV1
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
            return Ok(data);
        }

        // ──── V2 ──────────────────────────────────────────────────────────────
        [HttpGet("api/v2/WeatherForecast")]
        [ApiExplorerSettings(GroupName = "v2")]
        public IActionResult GetV2()
        {
            var data = Enumerable.Range(1, 5).Select(i => new WeatherForecastV2
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                TemperatureC = Random.Shared.Next(-20, 55),
                TemperatureF = 32 + (int)(1.8 * Random.Shared.Next(-20, 55)),
                WeatherSummary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
            return Ok(data);
        }
    }
}
