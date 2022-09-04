using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Service.Products;

namespace RedAndWhite.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IProductsService, ProductsService>();

            return services;
        }
    }
}