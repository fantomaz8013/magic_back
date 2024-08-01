using Magic.DAL.Configurations;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Magic.DAL
{
    public class DataBaseContext : DbContext
    {
        private const string PUBLIC = "public";

        public DbSet<Log> Log { get; set; }
        public DbSet<User> User { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(PUBLIC);

            modelBuilder.ApplyConfiguration(new LogConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
