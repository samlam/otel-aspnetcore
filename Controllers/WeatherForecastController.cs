﻿using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace core_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly Meter WeatherForecastMeter = new("WeatherForecastApi", "1.0");
        private static Counter<long> ForecastCounter = WeatherForecastMeter.CreateCounter<long>("weather_forecast_counter");


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            ForecastCounter.Add(1, new("location", "pasadena"), new("level", "low"));
            return Enumerable.Range(1, 15).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
