using Microsoft.EntityFrameworkCore;

namespace OpenTelemetryApm
{
    public class NewAzureSqlDbContext : DbContext
    {
        public DbSet<Models.Sport> Sports { get; set; }
        public DbSet<Models.City> Cities { get; set; }

        public NewAzureSqlDbContext(DbContextOptions<NewAzureSqlDbContext> options)
            : base(options)
        {

        }
    }
}
