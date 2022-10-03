using RedAndWhite.Domain.ValueObjects.Product;
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
            if(DoExistInBrands(brand)) return;

            this.Brands.Add(brand);
        }

        private bool DoExistInBrands(Brand brand)
        {
            return this.Brands.Any(b => b.Name.ToLower() == brand.Name.ToLower());
        }

        public void RemoveBrand(Brand brand)
        {
            var brandInProduct = this.Brands.FirstOrDefault(b => b.Name.ToLower() == brand.Name.ToLower());
            if (brandInProduct is default(Brand)) return;

            this.Brands.Remove(brandInProduct!);
        }

        public void AssignCategory(Category category)
        {
            if (DoExistInCategories(category)) return;

            Categories.Add(category);
        }

        private bool DoExistInCategories(Category category)
        {
            return this.Categories.Any(b => b.Name.ToLower() == category.Name.ToLower());
        }

        public void RemoveCategory(Category category)
        {
            var categoryInProduct = this.Categories.FirstOrDefault(b => b.Name.ToLower() == category.Name.ToLower());
            if (categoryInProduct is default(Category)) return;

            this.Categories.Remove(categoryInProduct!);
        }
    }
}
