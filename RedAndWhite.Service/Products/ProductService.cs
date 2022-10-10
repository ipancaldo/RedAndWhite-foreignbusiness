using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Products;
using RedAndWhite.Repository.Products;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Products
{
    public class ProductService : ServiceBase<Product, IProductRepository>, IProductService
    {
        private readonly IBrandDomainService _brandDomainService;
        private readonly ICategoryDomainService _categoryDomainService;

        public ProductService(IProductRepository repository,
                              IBrandDomainService brandDomainService,
                              ICategoryDomainService categoryDomainService,
                              IMapper mapper)
            : base(repository, mapper)
        {
            this._brandDomainService = brandDomainService;
            this._categoryDomainService = categoryDomainService;
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

        public void Delete(int id)
        {
            var product = GetProductById(id);
            IfNullThrowException(product);

            base.Repository.Delete(product);
            base.Repository.SaveChanges();
        }

        public void ModifyProperties(ModifyPropertiesProduct modifyPropertiesProduct)
        {
            var product = GetProductById(modifyPropertiesProduct.Id);
            IfNullThrowException(product);

            product.ModifyProperties(modifyPropertiesProduct);
            base.Repository.SaveChanges();
        }

        public void AssignBrand(AddOrRemoveProductBrandModel addOrRemoveProductBrandModel)
        {
            var product = GetProductById(addOrRemoveProductBrandModel.ProductId);
            IfNullThrowException(product);

            var brand = this._brandDomainService.GetById(base.Mapper.Map<GetBrandById>(addOrRemoveProductBrandModel));
            product.AssignBrand(brand);

            base.Repository.SaveChanges();
        }

        public void RemoveBrand(AddOrRemoveProductBrandModel addProductBrandModel)
        {
            var product = GetProductById(addProductBrandModel.ProductId);
            IfNullThrowException(product);

            var brand = this._brandDomainService.GetById(base.Mapper.Map<GetBrandById>(addProductBrandModel));
            product.RemoveBrand(brand);
            base.Repository.SaveChanges();
        }

        public void AssignCategory(AssignCategoryModel assignCategoryModel)
        {
            var category = this._categoryDomainService.GetByName(base.Mapper.Map<CategoryToGet>(assignCategoryModel));

            var product = GetProductById(assignCategoryModel.ProductId);
            IfNullThrowException(product);

            product.AssignCategory(category);

            base.Repository.SaveChanges();
        }

        public void RemoveCategory(RemoveCategoryFromProductModel removeCategoryFromProductModel)
        {
            var product = GetProductById(removeCategoryFromProductModel.ProductId);
            IfNullThrowException(product);

            var category = this._categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(removeCategoryFromProductModel));
            product.RemoveCategory(category);
            base.Repository.SaveChanges();
        }

        public List<Product> OrderBy()
        {
            return base.Repository.OrderBy(OrderByNameEvaluator()).ToList();
        }
        private Expression<Func<Product, string>> OrderByNameEvaluator() => product => product.Name;

        private void IfNullThrowException(Product product)
        {
            if (product is null)
                throw new Exception("Product don't exist");
        }
    }
}
