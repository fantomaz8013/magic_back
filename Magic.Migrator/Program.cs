using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System;
using Magic.DAL;

namespace Magic.Migrator
{
    public class Program : IDesignTimeDbContextFactory<DataBaseContext>
    {
        static async Task Main(string[] args)
        {
            Program program = new Program();

            await using (DataBaseContext dbContext = program.CreateDbContext())
            {
                await dbContext.Database.MigrateAsync();
            }

            Console.WriteLine("Migrate successfully!");
        }

        public DataBaseContext CreateDbContext(string[] args = null)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("DBConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>()
                .UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(Program).Namespace));

            return new DataBaseContext(optionsBuilder.Options);
        }
    }
}
