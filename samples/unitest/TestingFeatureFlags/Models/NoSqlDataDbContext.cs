using Microsoft.EntityFrameworkCore;

namespace TestingFeatureFlags.Models
{
    public class NoSqlDataDbContext : DbContext
    {
        public NoSqlDataDbContext(DbContextOptions<NoSqlDataDbContext> options)
        : base(options)
        {
        }

        public DbSet<One> OneItems { get; set; } = null!;
    }
}
