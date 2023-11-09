using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApmWithFeatureFlag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirQualityController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFbClient _fbClient;

        public AirQualityController(ILogger<WeatherForecastController> logger, IFbClient fbClient)
        {
            _logger = logger;
            _fbClient = fbClient;
        }

        [HttpGet]
        [Route("so2")]
        public string SO2()
        {
            if (_fbClient.StringVariation("air-quality-so2-algo", FbTrafficUser(), "regression") == "metnet")
            {
                throw new Exception("SO2 Algorithm Metnet is failure");
            }
            return "SO2 Good";

            static FbUser FbTrafficUser()
            {
                return FbUser.Builder($"traffic-{Guid.NewGuid()}").Build();
            }
        }

        [HttpGet]
        [Route("no2")]
        public string NO2()
        {
            if (_fbClient.StringVariation("air-quality-no2-algo", FbTrafficUser(), "regression") == "metnet")
            {
                throw new Exception("NO2 Algorithm Metnet is failure");
            }
            return "NO2 Good";
        }

        [HttpGet]
        [Route("pm10")]
        public string PM10()
        {
            if (_fbClient.StringVariation("air-quality-pm10-algo", FbTrafficUser(), "regression") == "metnet")
            {
                throw new Exception("PM10 Algorithm Metnet is failure");
            }
            return "NO2 Good";
        }

        [HttpGet]
        [Route("pm2p5")]
        public string PM2p5()
        {
            if (_fbClient.StringVariation("air-quality-pm2p5-algo", FbTrafficUser(), "regression") == "metnet")
            {
                throw new Exception("PM2p5 Algorithm Metnet is failure");
            }
            return "PM2p5 Good";
        }

        private FbUser FbTrafficUser()
        {
            return FbUser.Builder($"traffic-{Guid.NewGuid()}").Build();
        }
    }
}
