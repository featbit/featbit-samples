using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata.Ecma335;
using TestingFeatureFlags.Services;

namespace TestingFeatureFlags.XUnit
{
    public class DataServiceTests
    {
        [Fact]
        public void Test1()
        {

        }

        [Theory]
        public void ReadDataOneAsyncTest(string ffValue)
        {
            var mock = new Mock<IFbClient>();
            var fbUser = FbUser.Builder($"not-important").Build();
            mock.Setup(fb => fb.StringVariation("data-one-migration", fbUser, "")).Returns(ffValue);

            //using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            //ILogger logger = factory.CreateLogger("DataService");
            //var ds = new DataService(logger,  );
        }
    }
}