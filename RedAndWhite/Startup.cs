using RedAndWhite.Data;
using RedAndWhite.Infrastructure;
using RedAndWhite.Repository;
using RedAndWhite.Service;
using System.Reflection;

namespace RedAndWhite.UI
{
    public class Startup
    {
        public static WebApplication Initialize(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var assembliesToLoad = Assembly.GetExecutingAssembly()
                                           .GetReferencedAssemblies()
                                           .Select(a => Assembly.Load(a))
                                           .ToList();
            builder.Services.AddInfrastructure(assembliesToLoad.ToArray());

            builder.Services.AddData(builder.Configuration["ConnectionString"]);
            builder.Services.AddRepository();
            builder.Services.AddService();
        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
