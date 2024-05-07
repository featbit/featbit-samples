using FeatBit.Sdk.Server;
using TestingFeatureFlags.Models;
using TestingFeatureFlags.Repositories;
using TestingFeatureFlags.Utils;

namespace TestingFeatureFlags.Services
{
    public class DataService : IDataService
    {
        private readonly IOneRepository _oneRepository;
        private readonly IOneNoSqlRepository _oneNoSqlRepository;
        private readonly ILogger<DataService> _logger;
        private readonly IFbClient _fbClient;
        public DataService(
            ILogger<DataService> logger,
            IOneRepository oneRepository,
            IOneNoSqlRepository oneNoSqlRepository,
            IFbClient fbClient)
        {
            _logger = logger;
            _oneRepository = oneRepository;
            _oneNoSqlRepository = oneNoSqlRepository;
            _fbClient = fbClient;
        }

        public async Task<OneModel?> ReadDataOneAsync(string id)
        {
            // function for retriving data (name is one) from sql database
            var f1 = async () =>
            {
                var one = await _oneRepository.GetByIdAsync(id);
                return one == null ? null : new OneModel()
                {
                    Id = one.Id
                };
            };
            // function for retriving data (name is one) from noSql database
            var f2 = async () =>
            {
                var one = await _oneNoSqlRepository.GetByIdAsync(id);
                return one == null ? null : new OneModel()
                {
                    Id = one.Id
                };
            };
            // function for comparing the result of two functions above
            Action<OneModel?, OneModel?> aCompare = (r1, r2) =>
            {
                if(r2 == null)
                {
                    _logger.LogError($"Read One item in NoSql Database Failed");
                }
                if(r1?.Id != r2?.Id)
                {
                    _logger.LogError($"Item in noSql Database doesn't equal to Item in sql databse");
                }
            };

            // executing the migration process
            return await FbDbMigration<OneModel?>.MigrateAsync(f1, f2, _fbClient, "data-one-migration", aCompare);
        }
    }
}
