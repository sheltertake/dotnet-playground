using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FooApi.Controllers
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
        public WeatherForecastController(
            MyService myService,
            ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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
    }

    [ApiController]
    [Route("[controller]")]
    public class ProxyController : ControllerBase
    {


        private readonly MyService _myService;
        public ProxyController(
            MyService myService)
        {
            _myService = myService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await _myService.GetModelAsync();
            return Content(JsonConvert.SerializeObject(model), "application/json");
        }
    }

    public class MyService
    {
        private readonly HttpClient _client;

        public MyService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(nameof(MyService));
            _client.BaseAddress = new Uri("https://localhost:5001/");
        }

        public async Task<object> GetModelAsync()
        {
            var response = await _client.GetAsync($"WeatherForecast");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject(json);
            return model;
        }
    }
}
