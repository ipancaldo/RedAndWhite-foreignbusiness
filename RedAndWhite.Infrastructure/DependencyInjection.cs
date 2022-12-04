using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Infrastructure.Loaders;
using System.Reflection;

namespace RedAndWhite.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                           params Assembly[] assemblies)
        {
            services.AddAutoMapper(assemblies);
            services.AddScoped<IModelLoader, ModelLoader>();

            return services;
        }
    }
}
