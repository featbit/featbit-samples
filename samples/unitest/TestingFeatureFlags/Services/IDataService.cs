using TestingFeatureFlags.Models;

namespace TestingFeatureFlags.Services
{
    public interface IDataService
    {
        Task<DataModelOne> ReadDataOneAsync(string id);
    }
}
