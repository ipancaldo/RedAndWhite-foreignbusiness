using RedAndWhite.Data;
using RedAndWhite.Infrastructure;
using RedAndWhite.Repository;
using RedAndWhite.Service;
using System.Reflection;
using System.Text.Json.Serialization;

namespace RedAndWhite.API
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
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
