﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetryApm.Models;

namespace OpenTelemetryApm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        public VideosController() 
        { 
        }

        [HttpGet("GetSportsByCity/{cityId}")]
        public async Task<List<Sport>> GetSportsByCityId(int cityId)
        {
            Task.Delay(50).Wait();
            using var client = new HttpClient();

            var result = await client.GetAsync("http://localhost:5260/WeatherForecast");

            return new List<Sport>() { };
        }

        [HttpPost("GetSportsByCity")]
        public async Task<List<Sport>> GetSportsByCity()
        {
            Task.Delay((new Random()).Next(10, 50)).Wait();
            using var client = new HttpClient();

            var result = await client.GetAsync("http://localhost:5260/WeatherForecast");

            return new List<Sport>() { };
        }
    }
}