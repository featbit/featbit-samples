using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.FeatureManagement.Mvc;

namespace MFeatureManagement.Controllers
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
        private readonly IFeatureManager _featureManager;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IFeatureManager featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;
        }

        [FeatureGate("TrueFeatureFlag")]
        //[FeatureGate("FalseFeatureFlag")]
        //[FeatureGate("TargetingFeatureFlag")]
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        }

        [HttpGet("Get2")]
        public async Task<IActionResult> Get2()
        {
            TargetingContext targetingContext = new TargetingContext
            {
                UserId = "Jeff",
                Groups = ["Ring1", "group2"]
            };
            if (await _featureManager.IsEnabledAsync("TargetingFeatureFlag", targetingContext))
            {
                return Ok(true);
            }
            return StatusCode(StatusCodes.Status403Forbidden);
        }


        [FeatureGate("CombinationFeatureFlag1")]
        [HttpGet("Get3")]
        public async Task<IActionResult> Get3()
        {
            TargetingContext targetingContext = new TargetingContext
            {
                UserId = "Jeff",
                Groups = ["Ring1", "group2"]
            };
            if (await _featureManager.IsEnabledAsync("CombinationFeatureFlag1", targetingContext))
            {
                return Ok(true);
            }
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [FeatureGate("CustomFilterFeatureFlag")]
        [HttpGet("CustomFilter1")]
        public async Task<IActionResult> CustomFilter1()
        {
            return Ok(true);
        }

        [FeatureGate("RealCustomFilterFeatureFlag")]
        [HttpGet("CustomFilter2")]
        public async Task<IActionResult> CustomFilter2()
        {
            return Ok(true);
        }

        [FeatureGate("FeatBitFilterFeatureFlag")]
        [HttpGet("FeatBit")]
        public async Task<IActionResult> FeatBit()
        {
            return Ok(true);
        }
    }
}
