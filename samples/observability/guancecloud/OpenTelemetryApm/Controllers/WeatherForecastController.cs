using Microsoft.AspNetCore.Mvc;
using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;

namespace OpenTelemetryApm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFbClient _fbClient;
        private readonly ActivitySource activitySource;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly OldDbContext _oldDbContext;


        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, 
            IFbClient fbClient, 
            Instrumentation instrumentation,
            OldDbContext oldDbContext)
        {
            _logger = logger;
            _fbClient = fbClient;
            this.activitySource = instrumentation.ActivitySource;
            _oldDbContext = oldDbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("WeatherForecastController:Get");


            var user = FbUser.Builder("user-key-00000000000001").Name("user-00000000000001").Build();
            if (_fbClient.BoolVariation("feature-a", user, false) == true)
            {
                using (var activity = this.activitySource.StartActivity("FeatBit::feature-a::true::read data from new database"))
                {
                    //var span = Tracer.CurrentSpan;
                    //span.SetAttribute("feature-flags", $"feature-a:{true}");

                    using (var sqlActivity = this.activitySource.StartActivity("Sql Query : Get Sports From Database"))
                    {
                        var sports = _oldDbContext.Sports.ToList();
                        //activity?.SetStatus(ActivityStatusCode.Error);
                    }

                    using (var sqlActivity = this.activitySource.StartActivity("Sql Query : Get Cities From Database"))
                    {
                        Task.Delay(100).Wait();
                        //var sports = _oldDbContext.Cities.ToList();
                        sqlActivity?.SetStatus(ActivityStatusCode.Error, "Query to new Database Timeout");
                    }

                    //throw new Exception("An error has detected when running new feature a");
                }
            }

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