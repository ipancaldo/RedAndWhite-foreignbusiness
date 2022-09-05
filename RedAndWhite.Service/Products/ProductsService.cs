using RedAndWhite.Domain;
using RedAndWhite.Repository.Products;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Products
{
    public class ProductsService : ServiceBase<Product, IProductsRepository>, IProductsService
    {
        public ProductsService(IProductsRepository repository) 
            : base(repository)
        {
        }

        public Product GetProductById(int id)
        {
            return base.Repository.GetEntityByCriteria(GetById(id));
        }
        private Expression<Func<Product, bool>> GetById(int id) => user => user.Id == id;
    }
}
