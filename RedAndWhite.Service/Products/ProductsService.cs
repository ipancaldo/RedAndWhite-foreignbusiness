using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Repository.Products;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Products
{
    public class ProductsService : ServiceBase<Product, IProductsRepository>, IProductsService
    {
        private readonly IBrandDomainService _brandDomainService;

        public ProductsService(IProductsRepository repository,
                               IBrandDomainService brandDomainService) 
            : base(repository)
        {
            this._brandDomainService = brandDomainService;
        }

        public Product GetProductById(int id)
        {
            return base.Repository.GetEntityByCriteria(GetById(id));
        }
        private Expression<Func<Product, bool>> GetById(int id) => user => user.Id == id;

        public void AssignBrand(string brandName, int productId)
        {
            var brand = this._brandDomainService.GetOrCreateBrandByName(brandName);
            var product = GetProductById(productId);
            product.AssignBrand(brand);

            base.Repository.SaveChanges();
        }
    }
}
