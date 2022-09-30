using RedAndWhite.Domain.ValueObjects.Product;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [Required, MaxLength(150)]
        public string Name { get; private set; }

        [Required, MaxLength(200)]
        public string Description { get; private set; }

        public byte[]? Image { get; set; }

        public virtual List<Category> Categories { get; private set; }

        public virtual List<Brand> Brands { get; private set; }

        public void Create(NewProduct newProduct)
        {
            this.Name = newProduct.Name;
            this.Description = newProduct.Description;
        }

        public void ModifyProperties(ModifyPropertiesProduct modifyPropertiesProduct)
        {
            if (this.Name.ToLower() != modifyPropertiesProduct.Name.ToLower() &&
                !string.IsNullOrEmpty(modifyPropertiesProduct.Name))
                this.Name = modifyPropertiesProduct.Name;

            if (this.Description.ToLower() != modifyPropertiesProduct.Description.ToLower() &&
                !string.IsNullOrEmpty(modifyPropertiesProduct.Description))
                this.Description = modifyPropertiesProduct.Description;

            if (this.Image != modifyPropertiesProduct.Image &&
                modifyPropertiesProduct.Image is not null)
                this.Image = modifyPropertiesProduct.Image;
        }

        public void AssignBrand(Brand brand)
        {
            var isBrandInProduct = this.Brands.Any(b => b.Name == brand.Name);
            if (isBrandInProduct) return;

            Brands.Add(brand);
        }

        public void AddBrand(Brand brand)
        {
            var isBrandAlreadyAssigned = this.Brands.Any(b => b.Name == brand.Name);
            if(isBrandAlreadyAssigned) return;

            this.Brands.Add(brand);
        }

        public void RemoveBrand(Brand brand)
        {
            var brandInProduct = this.Brands.FirstOrDefault(b => b.Name == brand.Name);
            if (brandInProduct is default(Brand)) return;

            this.Brands.Remove(brandInProduct!);
        }
    }
}
