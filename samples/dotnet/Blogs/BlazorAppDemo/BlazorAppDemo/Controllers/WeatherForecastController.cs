using BlazorAppDemo.FeatureFlags;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {
        }

        [FeatureGate("air-quality-so2-algo", defaultValue: "regression", passedValues: ["metnet"])]
        [HttpGet]
        [Route("AirQualitySO2Algo")]
        public double AirQualitySO2Algo(int day, string areaCode)
        {
            return (new Random()).NextDouble();
        }
    }
}
