using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects;
using RedAndWhite.Model.Products;
using RedAndWhite.Repository.Products;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Products
{
    public class ProductsService : ServiceBase<Product, IProductsRepository>, IProductsService
    {
        private readonly IBrandDomainService _brandDomainService;

        public ProductsService(IProductsRepository repository,
                               IBrandDomainService brandDomainService,
                               IMapper mapper) 
            : base(repository, mapper)
        {
            this._brandDomainService = brandDomainService;
        }

        public Product GetProductById(int id)
        {
            return base.Repository.GetEntityByCriteria(GetByIdExpression(id));
        }
        private Expression<Func<Product, bool>> GetByIdExpression(int id) => user => user.Id == id;

        public void Create(NewProductModel newProductModel)
        {
            var product = base.Repository.GetEntityByCriteria(GetByNameExpression(newProductModel.Name));
            if (product is not null)
                return;

            this.Aggregate.Create(base.Mapper.Map<NewProduct>(newProductModel));
            base.Repository.Add(this.Aggregate);
            base.Repository.SaveChanges();
        }
        private Expression<Func<Product, bool>> GetByNameExpression(string productName) => product => product.Name == productName;

        public void AssignBrand(string brandName, int productId)
        {
            var brand = this._brandDomainService.GetOrCreateBrandByName(brandName);
            var product = GetProductById(productId);
            product.AssignBrand(brand);

            base.Repository.SaveChanges();
        }
    }
}
