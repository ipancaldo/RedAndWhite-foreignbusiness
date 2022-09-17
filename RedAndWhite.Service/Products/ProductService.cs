using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Products;
using RedAndWhite.Repository.Products;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Products
{
    public class ProductService : ServiceBase<Product, IProductRepository>, IProductService
    {
        private readonly IBrandDomainService _brandDomainService;

        public ProductService(IProductRepository repository,
                               IBrandDomainService brandDomainService,
                               IMapper mapper)
            : base(repository, mapper)
        {
            this._brandDomainService = brandDomainService;
        }

        public Product GetProductById(int id)
        {
            return base.Repository.GetEntityByCriteria(GetByIdEvaluator(id));
        }
        private Expression<Func<Product, bool>> GetByIdEvaluator(int id) => user => user.Id.Equals(id);

        public void Create(NewProductModel newProductModel)
        {
            var product = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newProductModel.Name));
            if (product is not null)
                throw new Exception("Product already exist.");

            this.Aggregate.Create(base.Mapper.Map<NewProduct>(newProductModel));
            base.Repository.Add(this.Aggregate);
            base.Repository.SaveChanges();
        }
        private Expression<Func<Product, bool>> GetByNameEvaluator(string productName) => product => product.Name.ToLower() == productName.ToLower();

        public void Modify(ModifyPropertiesProduct modifyPropertiesProduct)
        {
            var product = GetProductById(modifyPropertiesProduct.Id);
            if (product is null)
                throw new Exception("Product don't exist");

            product.ModifyProperties(modifyPropertiesProduct);
            base.Repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = GetProductById(id);
            if (product is null)
                throw new Exception("Product don't exist.");

            base.Repository.Delete(product);
            base.Repository.SaveChanges();
        }

        public void AssignBrand(string brandName, int productId)
        {
            var brand = this._brandDomainService.GetOrCreateBrandByName(new NewBrand(brandName));
            var product = GetProductById(productId);
            product.AssignBrand(brand);

            base.Repository.SaveChanges();
        }
    }
}
