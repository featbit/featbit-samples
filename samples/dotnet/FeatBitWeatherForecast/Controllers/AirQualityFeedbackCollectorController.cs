using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApmWithFeatureFlag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirQualityFeedbackCollectorController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFbClient _fbClient;

        public AirQualityFeedbackCollectorController(ILogger<AirQualityFeedbackCollectorController> logger, IFbClient fbClient)
        {
            _logger = logger;
            _fbClient = fbClient;
        }

        [HttpPost]
        [Route("pm10")]
        public void PM10TemperateureFeedBack([FromBody] FeedBackModel param)
        {
            var user = FbUser.Builder($"user-{param.UserKey}").Name($"name-{param.UserKey}").Build();
            _fbClient.Track(user, "pm10-forecast-temperature-error", param.Error);
        }


        private FbUser FbTrafficUser()
        {
            return FbUser.Builder($"traffic-{Guid.NewGuid()}").Build();
        }
    }

    public class FeedBackModel
    {
        public string UserKey { get; set; }
        /// <summary>
        /// observed temperature - forecasted temperature
        /// </summary>
        public double Error { get; set;}
    }
}
