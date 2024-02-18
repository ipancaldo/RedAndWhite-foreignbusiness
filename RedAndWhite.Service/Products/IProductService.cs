using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Products;
using RedAndWhite.Model.Shared;

namespace RedAndWhite.Service.Products
{
    public interface IProductService : IServiceBase<Product>
    {
        List<ProductModel> GetAllProducts();

        Task<Product> GetById(int id);

        Task<List<ProductModel>> GetByCategoryId(GetProductByIdModel categoryModel);

        Task<ResultDTO<Product>> Create(NewProductModel newProductModel);

        Task Delete(int id);

        Task Update(ModifyPropertiesProduct modifyPropertiesProduct);

        Task AssignBrand(AddOrRemoveProductBrandModel addOrRemoveProductBrandModel);

        Task RemoveBrand(AddOrRemoveProductBrandModel addProductBrandModel);

        Task AssignCategory(AssignCategoryModel assignCategoryModel);

        Task RemoveCategory(RemoveCategoryFromProductModel removeCategoryFromProductModel);

        List<Product> OrderBy();
    }
}
