using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.Extensions.Logging;
using Moq;
using TestingFeatureFlags.Models;
using TestingFeatureFlags.Repositories;
using TestingFeatureFlags.Services;

namespace TestingFeatureFlags.XUnit
{
    public class DataServiceTests
    {
        [Fact]
        public void Test1()
        {

        }

        [Theory, CombinatorialData]
        public async void ReadDataOneAsyncTest(
            [CombinatorialValues("ReadFromOldDbOnly", "ReadFromNewDbOnly", "ReadFromOldAndNewDb")]string ffValue,
            [CombinatorialValues("not-important")] string oneId)
        {
            var mockFbClient = new Mock<IFbClient>();
            mockFbClient.Setup(fb => fb.StringVariation("data-one-migration", It.IsAny<FbUser>(), "")).Returns(ffValue);

            var mockNoSqlRepo = new Mock<IOneNoSqlRepository>();
            mockNoSqlRepo.Setup(r => r.GetByIdAsync(oneId)).ReturnsAsync(new OneNoSql() { Id = oneId });

            var mockSqlRepo = new Mock<IOneRepository>();
            mockSqlRepo.Setup(r => r.GetByIdAsync(oneId)).ReturnsAsync(new One() { Id = oneId });

            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<DataService> logger = factory.CreateLogger<DataService>();

            var ds = new DataService(logger, mockSqlRepo.Object, mockNoSqlRepo.Object, mockFbClient.Object);
            var ro = await ds.ReadDataOneAsync(oneId);

            Assert.Equal(oneId, ro?.Id);
        }
    }
}