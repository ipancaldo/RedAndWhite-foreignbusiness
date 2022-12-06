using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Categories;
using RedAndWhite.Model.Products;

namespace RedAndWhite.Service.Products
{
    public interface IProductService : IServiceBase<Product>
    {
        List<ProductModel> GetAllProducts();

        Product GetProductById(int id);

        List<ProductModel> GetByCategory(GetProductsByCategoryModel categoryModel);

        void Create(NewProductModel newProductModel);

        void Delete(int id);

        void ModifyProperties(ModifyPropertiesProduct modifyPropertiesProduct);

        void AssignBrand(AddOrRemoveProductBrandModel addOrRemoveProductBrandModel);

        void RemoveBrand(AddOrRemoveProductBrandModel addProductBrandModel);

        void AssignCategory(AssignCategoryModel assignCategoryModel);

        void RemoveCategory(RemoveCategoryFromProductModel removeCategoryFromProductModel);

        List<Product> OrderBy();
    }
}
