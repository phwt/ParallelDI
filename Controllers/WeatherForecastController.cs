using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParallelDI.Service;

namespace ParallelDI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly GuidService _guidService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, GuidService guidService)
        {
            _logger = logger;
            _guidService = guidService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string[] items = new[] { "Alpha", "Bravo", "Charlie", "Delta" };
            var results = new List<string>() { };

            var bag = new ConcurrentBag<object>();
            var tasks = items.Select(async item =>
            {
                _guidService.CustomID = $"{item}ID";
                await Task.Delay(1000);
                results.Add($"{_guidService.GetID()} - {_guidService.CustomID} - {item}");
            });
            await Task.WhenAll(tasks);

            return new OkObjectResult(results);
        }
    }
}
