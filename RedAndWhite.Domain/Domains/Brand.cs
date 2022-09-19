using RedAndWhite.Domain.ValueObjects.Brand;
using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Brand : BaseDomain
    {
        public Brand()
        {
            this.Products = new List<Product>();
            this.Categories = new List<Category>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; private set; }

        public virtual List<Product> Products { get; set; }

        public virtual List<Category> Categories { get; set; }

        public void Create(NewBrand newBrand)
        {
            this.Name = newBrand.Name;
        }

        public void Modify(ModifyPropertiesBrand modifyPropertiesBrand)
        {
            this.Name = modifyPropertiesBrand.Name;
        }
    }
}
