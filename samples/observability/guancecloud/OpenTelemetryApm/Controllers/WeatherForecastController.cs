using Microsoft.AspNetCore.Mvc;
using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using OpenTelemetryApm.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;

namespace OpenTelemetryApm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFbClient _fbClient;
        private readonly IDistributedCache _cache;
        private readonly ActivitySource activitySource;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly OldDbContext _oldDbContext;
        private readonly NewAzureSqlDbContext _newDbContext;


        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, 
            IFbClient fbClient, 
            IDistributedCache cache,
            Instrumentation instrumentation,
            OldDbContext oldDbContext,
            NewAzureSqlDbContext newDbContext)
        {
            _logger = logger;
            _fbClient = fbClient;
            _cache = cache;
            this.activitySource = instrumentation.ActivitySource;
            _oldDbContext = oldDbContext;
            _newDbContext = newDbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("WeatherForecastController:Get");


            var user = FbUser.Builder("user-key-00000000000001").Name("user-00000000000001").Build();


            var tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {

                if (_fbClient.BoolVariation("read-sport-olddb", user, true) == true)
                {
                    using (var activity = this.activitySource.StartActivity("FeatBit/read-sport-olddb/true"))
                    {
                        using (var sqlActivity = this.activitySource.StartActivity("mssql"))
                        {
                            Task.Delay((new Random()).Next(200, 300)).Wait();
                            //var sports = _oldDbContext.Sports.ToList();
                            //activity?.SetStatus(ActivityStatusCode.Error);
                        }
                    }
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                if (_fbClient.BoolVariation("read-sport-newdb", user, true) == true)
                {
                    using (var activity = this.activitySource.StartActivity("FeatBit/read-sport-newdb/true"))
                    {
                        using (var sqlActivity = this.activitySource.StartActivity("Sql Query: Get Sports by City From New Database"))
                        {
                            var delay = (new Random()).Next(200, 500);
                            Task.Delay(delay).Wait();
                            //if(delay > 300)
                            //{
                            Task.Delay((new Random()).Next(300, 500)).Wait();
                            var span = Tracer.CurrentSpan;
                            span.SetAttribute("error_type", $"Sql Query Error: Query to new Database Timeout when getting sports by city from new database");
                            sqlActivity?.SetStatus(ActivityStatusCode.Error, "Query to new Database Timeout");
                            //}
                            //else
                            //{
                            //    Task.Delay((new Random()).Next(200, 300)).Wait();
                            //    //var sports = _oldDbContext.Sports.ToList();
                            //}
                        }
                    }
                }
            }));
            await Task.WhenAll(tasks);

            // https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Api#introduction-to-opentelemetry-net-tracing-api
            using (var activity = this.activitySource.StartActivity("Http Post Demo"))
            {
                activity?.SetTag("http.method", "POST");
                if (activity != null && activity.IsAllDataRequested == true)
                {
                    activity.SetTag("http.url", "http://www.mywebsite.com");
                }

                activity?.AddEvent(new ActivityEvent("sample activity event."));
            }

            var sports = await _newDbContext.Sports.Where(p => p.Name != null).ToListAsync();

            var currentTimeUTC = DateTime.UtcNow.ToString();
            byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));
            await _cache.SetAsync("cachedTimeUTC", encodedCurrentTimeUTC, options);
            await _cache.GetAsync("cachedTimeUTC");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}