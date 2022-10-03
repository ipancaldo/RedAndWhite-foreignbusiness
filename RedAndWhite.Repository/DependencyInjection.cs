using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Repository.Brands;
using RedAndWhite.Repository.Categories;
using RedAndWhite.Repository.Products;

namespace RedAndWhite.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
