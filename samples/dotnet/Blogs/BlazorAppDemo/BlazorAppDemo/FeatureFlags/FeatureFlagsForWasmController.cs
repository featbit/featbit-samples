using BlazorAppDemo.FeatureFlags;
using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureFlagsForWasmController : ControllerBase
    {
        private readonly IFbClient _fbClient;

        public FeatureFlagsForWasmController(IFbClient fbClient)
        {
            _fbClient = fbClient;
        }

        //[FeatureFlags(["weather-page"])]
        [HttpGet]
        [Route("BoolVariation")]
        public bool BoolVariation(string flagKey)
        {
            var user = FbUser.Builder(Guid.NewGuid().ToString()).Build();
            return _fbClient.BoolVariation(flagKey, user, false);
        }
    }
}
