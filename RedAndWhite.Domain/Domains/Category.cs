using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Domain.ValueObjects.Product;
using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Category : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual List<Brand> Brands { get; set; }

        public void Create(NewCategory newCategory)
        {
            Name = newCategory.CategoryName;
        }
    }
}
