using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Distributed;
using OpenTelemetry.Trace;
using OpenTelemetryApm.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace OpenTelemetryApm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly IFbClient _fbClient;
        private readonly IDistributedCache _cache;
        private readonly ActivitySource activitySource;
        private readonly ILogger<SportsController> _logger;
        private readonly OldDbContext _oldDbContext;
        private readonly NewAzureSqlDbContext _newDbContext;

        public SportsController(
            ILogger<SportsController> logger,
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
            //using var client = new HttpClient();

            //int cityId = (new Random()).Next(810503, 1092001);

            //var result = await client.GetAsync($"http://localhost:5260/api/Sports/GetSportsByCity/{cityId}");


            _logger.LogInformation("GetSportsByCity");


            var user = FbUser.Builder("user-key-00000000000001").Name("user-00000000000001").Build();


            var tasks = new List<Task>();
            tasks.Add(Task.Run(async () =>
            {
                if (_fbClient.BoolVariation("read-sport-olddb", user, true) == true)
                {
                    using (var activity = this.activitySource.StartActivity("fb-feature-flags/read-sport-olddb/true"))
                    {
                        var spanF = Tracer.CurrentSpan;
                        spanF.SetAttribute("service_type", "fb-feature-flag");
                        var sports = await _newDbContext.Sports.Where(p => p.Name != null).ToListAsync();
                    }
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                if (_fbClient.BoolVariation("read-sport-newdb", user, true) == true)
                {
                    using (var activity = this.activitySource.StartActivity("fb-feature-flags/read-sport-newdb/true"))
                    {
                        var spanF = Tracer.CurrentSpan;
                        spanF.SetAttribute("service_type", "fb-feature-flag");

                        using (var sqlActivity = this.activitySource.StartActivity("Sql Query: Get Sports by City From New Database"))
                        {
                            var span = Tracer.CurrentSpan;
                            span.SetAttribute("db_system", "mssql");
                            //var delay = (new Random()).Next(200, 500);
                            //Task.Delay(delay).Wait();
                            //if (delay > 325)
                            //{
                                Task.Delay((new Random()).Next(300, 500)).Wait();
                                span.SetAttribute("error_type", $"Sql Query Error: Query to new Database Timeout when getting sports by city from new database");
                                sqlActivity?.SetStatus(ActivityStatusCode.Error, "Query to new Database Timeout");
                            //}
                            //else
                            //{
                            //    Task.Delay((new Random()).Next(100, 150)).Wait();
                            //}
                        }
                        Task.Delay((new Random()).Next(200, 500)).Wait();
                    }
                }
            }));
            await Task.WhenAll(tasks);

            //using (var activity = this.activitySource.StartActivity("Http Post Demo"))
            //{
            //    activity?.SetTag("http.method", "POST");
            //    if (activity != null && activity.IsAllDataRequested == true)
            //    {
            //        activity.SetTag("http.url", "http://www.mywebsite.com");
            //    }

            //    activity?.AddEvent(new ActivityEvent("sample activity event."));
            //}

            //var sports = await _newDbContext.Sports.Where(p => p.Name != null).ToListAsync();

            //var currentTimeUTC = DateTime.UtcNow.ToString();
            //byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
            //var options = new DistributedCacheEntryOptions()
            //    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
            //await _cache.SetAsync("cachedTimeUTC", encodedCurrentTimeUTC, options);
            //await _cache.GetAsync("cachedTimeUTC");

            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();

            return new List<Sport>() { };
        }


        [HttpPost("GetSportsByCityWithoutBug")]
        public async Task<List<Sport>> GetSportsByCityWithoutBug()
        {
            List<string> activityNames = new List<string> { 
                "api/AuditLogs/GetByOperator",
                "Document Service",
                "Queue for aggregators",
                "GDPR Verification 1022",
                "fb-feature-flags/hotmap/true",
                "fb-feature-flags/gdpr-verify/true",
                "fb-feature-flags/storemap-na/true",
                "api/storemap/cuisine",
                "api/storemap/food",
                "api/growthbook/asia",
                "api/growthbook/africa",
                "api/growthbook/europe",
            };
            for(int i = 0; i < activityNames.Count; i++)
            {
                if((new Random()).Next(100, 200) > 130)
                {
                    using (var activity = this.activitySource.StartActivity("fb-feature-flags/read-sport-newdb/true"))
                    {
                        Task.Delay((new Random()).Next(10, 20)).Wait();

                        if (activityNames[i] == "Queue for aggregators")
                        {
                            var span = Tracer.CurrentSpan;
                            span.SetAttribute("service_type", "queue-messaging");
                        }
                        else if (activityNames[i] == "Document Service")
                        {
                            var span = Tracer.CurrentSpan;
                            span.SetAttribute("service_type", "document-service");
                        }
                        else if (activityNames[i].StartsWith("fb-feature-flags"))
                        {
                            var span = Tracer.CurrentSpan;
                            span.SetAttribute("service_type", "feature-flag");
                        }
                    }
                }
            }

            return new List<Sport>() { };
        }

    }
}