using RedAndWhite.Domain;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class ProductModel : IMapFrom<Product>
    {
        public ProductModel()
        {
            Categories = new List<Category>();
            Brands = new List<Brand>();
        }

        public int Id { get; set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string? Image { get; set; }

        public virtual List<Category> Categories { get; private set; }

        public virtual List<Brand> Brands { get; private set; }
    }
}
