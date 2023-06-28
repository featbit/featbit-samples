using Microsoft.EntityFrameworkCore;

namespace OpenTelemetryApm
{
    public class OldDbContext : DbContext
    {
        public DbSet<Models.Sport> Sports { get; set; }
        public DbSet<Models.City> Cities { get; set; }

        public OldDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=C:\\Code\\featbit\\featbit-samples\\samples\\observability\\guancecloud\\OpenTelemetryApm\\old.db");
        }
    }
}
