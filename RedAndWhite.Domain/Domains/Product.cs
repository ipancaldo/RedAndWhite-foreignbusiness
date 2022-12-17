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

        public string? Image { get; set; }

        public virtual List<Category> Categories { get; private set; }

        public virtual List<Brand> Brands { get; private set; }

        public void Create(NewProduct newProduct)
        {
            Name = newProduct.Name;
            Description = newProduct.Description;
        }

        //TODO: Modify this for new properties
        public void ModifyProperties(ModifyPropertiesProduct modifyPropertiesProduct)
        {
            if (modifyPropertiesProduct.Name.Count() < 2)
                throw new Exception($"The name of the producy cannot be less than 2 characters.");

            if (Name.ToLower() != modifyPropertiesProduct.Name.ToLower() &&
                !string.IsNullOrEmpty(modifyPropertiesProduct.Name))
                Name = modifyPropertiesProduct.Name;

            if (Description.ToLower() != modifyPropertiesProduct.Description.ToLower() &&
                !string.IsNullOrEmpty(modifyPropertiesProduct.Description))
                Description = modifyPropertiesProduct.Description;

            //if (this.Image != modifyPropertiesProduct.Image &&
            //    modifyPropertiesProduct.Image is not null)
            //    this.Image = modifyPropertiesProduct.Image;
        }

        public void AssignBrand(Brand brand)
        {
            if(DoExistInBrands(brand)) return;

            Brands.Add(brand);
        }

        private bool DoExistInBrands(Brand brand)
        {
            return Brands.Any(b => b.Name.ToLower() == brand.Name.ToLower());
        }

        public void RemoveBrand(Brand brand)
        {
            var brandInProduct = Brands.FirstOrDefault(b => b.Name.ToLower() == brand.Name.ToLower());
            if (brandInProduct is default(Brand)) return;

            Brands.Remove(brandInProduct!);
        }

        public void AssignCategory(Category category)
        {
            if (DoExistInCategories(category)) return;

            Categories.Add(category);
        }

        private bool DoExistInCategories(Category category)
        {
            return Categories.Any(b => b.Name.ToLower() == category.Name.ToLower());
        }

        public void RemoveCategory(Category category)
        {
            var categoryInProduct = Categories.FirstOrDefault(b => b.Name.ToLower() == category.Name.ToLower());
            if (categoryInProduct is default(Category)) return;

            Categories.Remove(categoryInProduct!);
        }
    }
}
