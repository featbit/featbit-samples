using TestingFeatureFlags.Models;

namespace TestingFeatureFlags.Repositories
{
    public class OneNoSqlRepository : IDisposable, IOneNoSqlRepository
    {
        private readonly NoSqlDataDbContext _dbContext;

        public OneNoSqlRepository(NoSqlDataDbContext context)
        {
            _dbContext = context;
        }

        public async Task<OneNoSql> GetByIdAsync(string id)
        {
            //return await _dbContext.OneItems.FindAsync(id);
            await Task.Delay(500);
            return new OneNoSql
            {
                Id = id
            };
        }

        public async Task InsertAsync(OneNoSql one)
        {
            _dbContext.OneItems.Add(one);
            await _dbContext.SaveChangesAsync();
        }

        #region dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
