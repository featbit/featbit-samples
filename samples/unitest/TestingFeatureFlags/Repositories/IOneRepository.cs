using TestingFeatureFlags.Models;

namespace TestingFeatureFlags.Repositories
{
    public interface IOneRepository : IDisposable
    {
        Task<One> GetByIdAsync(string id);
        Task InsertAsync(One one);
    }
}
