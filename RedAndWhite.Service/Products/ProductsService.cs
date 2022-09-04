using RedAndWhite.Domain;
using RedAndWhite.Repository.Products;

namespace RedAndWhite.Service.Products
{
    public class ProductsService : ServiceBase<Product, IProductsRepository>, IProductsService
    {
        public ProductsService(IProductsRepository repository) 
            : base(repository)
        {
        }
    }
}
