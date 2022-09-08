using RedAndWhite.Domain;

namespace RedAndWhite.Service.Products
{
    public interface IProductsService : IServiceBase<Product>
    {
        Product GetProductById(int id);
        void AssignBrand(string brandName, int productId);
    }
}
