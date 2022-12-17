using RedAndWhite.Domain.ValueObjects.Brand;
using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Brand : BaseDomain
    {
        public Brand()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; private set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(250)]
        public string? Image { get; set; }

        [MaxLength(250)]
        public string? Website { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual List<Category> Categories { get; set; }

        public void Create(NewBrand newBrand)
        {
            Name = newBrand.BrandName;
        }

        public void Modify(ModifyPropertiesBrand modifyPropertiesBrand)
        {
            Name = modifyPropertiesBrand.Name;
        }
    }
}
