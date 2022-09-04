using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RedAndWhite.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RedAndWhiteContext>(options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));

            return services;
        }
    }
}
