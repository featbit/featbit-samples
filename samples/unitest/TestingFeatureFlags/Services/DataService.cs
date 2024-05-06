using FeatBit.Sdk.Server;
using TestingFeatureFlags.Models;
using TestingFeatureFlags.Utils;

namespace TestingFeatureFlags.Services
{
    public class DataService : IDataService
    {
        private readonly DataDbContext _dbContext;
        private readonly DataDbV2Context _dbV2Context;
        private readonly IFbClient _fbClient;
        public DataService(
            DataDbContext dbContext,
            DataDbV2Context dbV2Context,
            IFbClient fbClient)
        {
            _dbContext = dbContext;
            _dbV2Context = dbV2Context;
            _fbClient = fbClient;
        }

        public async Task<DataModelOne> ReadDataOneAsync(string id)
        {
            await Task.Delay(1000);

            if (_fbClient.FlagValue("data-one-migration", "none") == "read-only")
            {
                // simulate read operation
                await Task.Delay(1000);

                // simulate read compare operation
                await Task.Delay(1000);
            }

            return new DataModelOne()
            {
                Id = id
            };
        }
    }
}
