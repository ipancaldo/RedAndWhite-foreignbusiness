using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects;
using RedAndWhite.Model.Products;

namespace RedAndWhite.Service.Products
{
    public interface IProductService : IServiceBase<Product>
    {
        Product GetProductById(int id);

        void Create(NewProductModel newProductModel);

        void EditProduct(ModifyPropertiesProduct modifyPropertiesProduct);

        void AssignBrand(string brandName, int productId);
    }
}
