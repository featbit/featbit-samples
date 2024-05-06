using Microsoft.EntityFrameworkCore;

namespace TestingFeatureFlags.Models
{
    public class SqlDataDbContext : DbContext
    {
        public SqlDataDbContext(DbContextOptions<SqlDataDbContext> options)
        : base(options)
        {
        }

        public DbSet<One> OneItems { get; set; } = null!;
    }
}
