using TestingFeatureFlags.Models;

namespace TestingFeatureFlags.Repositories
{
    public interface IOneNoSqlRepository : IDisposable
    {
        Task<OneNoSql> GetByIdAsync(string id);
        Task InsertAsync(OneNoSql one);
    }
}
