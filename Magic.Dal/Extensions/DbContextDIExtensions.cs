using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Magic.DAL.Extensions
{
    public static class DbContextDIExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, string dbConnectionString)
        {
            services.AddDbContext<DataBaseContext>(builder =>
            {
                builder.UseNpgsql(dbConnectionString);
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}
