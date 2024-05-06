using TestingFeatureFlags.Models;

namespace TestingFeatureFlags.Repositories
{
    public class OneNoSqlRepository : IDisposable, IOneNoSqlRepository
    {
        private readonly SqlDataDbContext _dbContext;

        public OneNoSqlRepository(SqlDataDbContext context)
        {
            _dbContext = context;
        }

        public async Task<One> GetByIdAsync(string id)
        {
            //return await _dbContext.OneItems.FindAsync(id);
            await Task.Delay(500);
            return new One
            {
                Id = id
            };
        }

        public async Task InsertAsync(One one)
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
