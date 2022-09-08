using Microsoft.EntityFrameworkCore;
using RedAndWhite.Domain;

namespace RedAndWhite.Data
{
    public class RedAndWhiteContext : DbContext
    {
        public RedAndWhiteContext() { }

        public RedAndWhiteContext(DbContextOptions<RedAndWhiteContext> options) 
            : base(options)
        {

        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Information> Informations { get; set; }
    }
}
