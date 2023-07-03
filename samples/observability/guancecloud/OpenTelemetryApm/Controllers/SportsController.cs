﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetryApm.Models;

namespace OpenTelemetryApm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        public SportsController() 
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
            using var client = new HttpClient();

            int cityId = (new Random()).Next(810503, 1092001);

            var result = await client.GetAsync($"http://localhost:5260/api/Sports/GetSportsByCity/{cityId}");

            return new List<Sport>() { };
        }


        [HttpPost("GetSportsByCityWithoutBug")]
        public async Task<List<Sport>> GetSportsByCityWithoutBug()
        {
            Task.Delay((new Random()).Next(200, 350)).Wait();
            using var client = new HttpClient();

            return new List<Sport>() { };
        }

    }
}