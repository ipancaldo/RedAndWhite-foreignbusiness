using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Products;

namespace RedAndWhite.Service.Products
{
    public interface IProductService : IServiceBase<Product>
    {
        Product GetProductById(int id);

        void Create(NewProductModel newProductModel);

        void Delete(int id);

        void ModifyProperties(ModifyPropertiesProduct modifyPropertiesProduct);

        void AddBrand(AddOrRemoveProductBrandModel addProductBrandModel);

        void RemoveBrand(AddOrRemoveProductBrandModel addProductBrandModel);

        void AssignBrand(string brandName, int productId);

        List<Product> OrderBy();
    }
}
