using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Product : BaseDomain
    {
        public Product()
        {
            Categories = new List<Category>();
            Brands = new List<Brand>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[]? Image { get; set; }

        public virtual List<Category> Categories { get; set; }

        public virtual List<Brand> Brands { get; set; }

        public void AssignBrand(Brand brand)
        {
            var isBrandInProduct = Brands.Any(b => b.Name == brand.Name);
            if (isBrandInProduct) return;

            Brands.Add(brand);
        }
    }
}
