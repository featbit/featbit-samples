using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApmWithFeatureFlag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFbClient _fbClient;

        public RainForecastController(ILogger<WeatherForecastController> logger, IFbClient fbClient)
        {
            _logger = logger;
            _fbClient = fbClient;
        }

        [HttpGet]
        public string Get()
        {
            if (_fbClient.StringVariation("rain-forecast-algo", FbTrafficUser(), "regression") == "metnet")
            {
                throw new Exception("Rain Forecast Algorithm Metnet is failure");
            }
            return "Rain Forecast Good";
        }

        private FbUser FbTrafficUser()
        {
            return FbUser.Builder($"traffic-{Guid.NewGuid()}").Build();
        }
    }
}
