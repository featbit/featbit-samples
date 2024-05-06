using Microsoft.EntityFrameworkCore;

namespace TestingFeatureFlags.Models
{
    public class DataDbV2Context : DbContext
    {
        public DataDbV2Context(DbContextOptions<DataDbV2Context> options)
        : base(options)
        {
        }

        public DbSet<DataModelOne> OneItems { get; set; } = null!;
    }
}
