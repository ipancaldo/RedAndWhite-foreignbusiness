using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Data;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Infrastructure.Mapping;
using RedAndWhite.Repository.Brands;
using RedAndWhite.Repository.Categories;
using RedAndWhite.Repository.Informations;
using RedAndWhite.Repository.Products;
using RedAndWhite.Service.Brands;
using RedAndWhite.Service.Categories;
using RedAndWhite.Service.Common;
using RedAndWhite.Service.Informations;
using RedAndWhite.Service.Products;
using System.Reflection;

namespace RedAndWhite.Test
{
    public class BaseTest : IClassFixture<ServiceProviderFixture>
    {
        protected readonly IServiceProvider _serviceProvider;

        public BaseTest(ServiceProviderFixture serviceProviderFixture)
        {
            _serviceProvider = serviceProviderFixture.ServiceProvider;
        }
    }

    public class ServiceProviderFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }

        public ServiceProviderFixture()
        {
            ServiceProvider = BootIoC();
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
        }

        private static ServiceProvider BootIoC()
        {
            var serviceCollection = new ServiceCollection();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            IMapper mapper = mapperConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);

            serviceCollection.AddData("data source=(local); initial catalog=RedAndWhite;Integrated Security=True");
            serviceCollection.AddScoped<IProductRepository, ProductRepository>();
            serviceCollection.AddScoped<IInformationService, InformationService>();
            serviceCollection.AddScoped<IProductService, ProductService>();
            serviceCollection.AddScoped<IResultVerifierService, ResultVerifierService>();
            RegisterAsImplementedInterfaces<CategoryService>(serviceCollection, ServiceLifetime.Scoped);
            RegisterAsImplementedInterfaces<BrandService>(serviceCollection, ServiceLifetime.Scoped);

            serviceCollection.AddScoped<IProductRepository, ProductRepository>();
            serviceCollection.AddScoped<IBrandRepository, BrandRepository>();
            serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddScoped<IInformationRepository, InformationRepository>();

            serviceCollection.AddScoped<IModelLoader, ModelLoader>();

            return serviceCollection.BuildServiceProvider();
        }

        private static void RegisterAsImplementedInterfaces<TService>(IServiceCollection services, ServiceLifetime lifetime)
        {
            var interfaces = typeof(TService).GetTypeInfo().ImplementedInterfaces
                                .Where(i => i != typeof(IDisposable) && i.IsPublic);

            foreach (var interfaceType in interfaces)
            {
                services.Add(new ServiceDescriptor(interfaceType, typeof(TService), lifetime));
            }
        }
    }
}