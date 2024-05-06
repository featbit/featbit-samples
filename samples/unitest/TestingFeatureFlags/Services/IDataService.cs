using TestingFeatureFlags.Models;

namespace TestingFeatureFlags.Services
{
    public interface IDataService
    {
        Task<OneModel?> ReadDataOneAsync(string id);
    }
}
