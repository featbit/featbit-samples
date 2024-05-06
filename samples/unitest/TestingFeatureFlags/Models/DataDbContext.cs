using Microsoft.EntityFrameworkCore;

namespace TestingFeatureFlags.Models
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options)
        : base(options)
        {
        }

        public DbSet<DataModelOne> OneItems { get; set; } = null!;
    }
}
