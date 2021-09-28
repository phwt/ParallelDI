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
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> _logger;
        private readonly GuidService _guidService;

        string[] items = new[] { "Alpha", "Bravo", "Charlie", "Delta" };

        public APIController(ILogger<APIController> logger, GuidService guidService)
        {
            _logger = logger;
            _guidService = guidService;
        }

        [HttpGet]
        public async Task<IActionResult> PostService()
        {
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

        [HttpPost]
        public async Task<IActionResult> PostLocal()
        {
            var results = new List<string>() { };

            var bag = new ConcurrentBag<object>();
            var tasks = items.Select(async item =>
            {
                var localGuidService = new GuidService();
                localGuidService.CustomID = $"{item}ID";
                await Task.Delay(1000);
                results.Add($"{localGuidService.GetID()} - {localGuidService.CustomID} - {item}");
            });
            await Task.WhenAll(tasks);

            return new OkObjectResult(results);
        }
    }
}
