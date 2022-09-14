using RedAndWhite.Domain;
using RedAndWhite.Model.Products;

namespace RedAndWhite.Service.Products
{
    public interface IProductsService : IServiceBase<Product>
    {
        Product GetProductById(int id);

        void Create(NewProductModel newProductModel);

        void AssignBrand(string brandName, int productId);
    }
}
