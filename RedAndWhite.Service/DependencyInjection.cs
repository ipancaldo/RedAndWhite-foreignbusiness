﻿using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Service.Brands;
using RedAndWhite.Service.Categories;
using RedAndWhite.Service.Common;
using RedAndWhite.Service.Informations;
using RedAndWhite.Service.Products;
using System.Reflection;

namespace RedAndWhite.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInformationService, InformationService>();
            services.RegisterAsImplementedInterfaces<BrandService>(ServiceLifetime.Scoped);
            services.RegisterAsImplementedInterfaces<CategoryService>(ServiceLifetime.Scoped);
            services.AddScoped<IResultVerifierService, ResultVerifierService>();

            return services;
        }

        private static void RegisterAsImplementedInterfaces<TService>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            var interfaces = typeof(TService).GetTypeInfo().ImplementedInterfaces
                                .Where(i => i != typeof(IDisposable) && (i.IsPublic));

            foreach (Type interfaceType in interfaces)
                services.Add(new ServiceDescriptor(interfaceType, typeof(TService), lifetime));
        }
    }
}