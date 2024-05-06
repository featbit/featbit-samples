using TestingFeatureFlags.Models;

namespace TestingFeatureFlags.Repositories
{
    public interface IOneNoSqlRepository : IDisposable
    {
        Task<One> GetByIdAsync(string id);
        Task InsertAsync(One one);
    }
}
