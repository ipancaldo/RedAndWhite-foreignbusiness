using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Categories;
using RedAndWhite.Model.Products;
using RedAndWhite.Repository.Products;
using RedAndWhite.Service.Common;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Products
{
    public class ProductService : ServiceBase<Product, IProductRepository>, IProductService
    {
        private readonly IBrandDomainService _brandDomainService;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IResultVerifier _resultVerifier;

        const string ProductType = "Product";

        public ProductService(IProductRepository repository,
                              IBrandDomainService brandDomainService,
                              ICategoryDomainService categoryDomainService,
                              IResultVerifier resultVerifier,
                              IMapper mapper)
            : base(repository, mapper)
        {
            this._brandDomainService = brandDomainService;
            this._categoryDomainService = categoryDomainService;
            this._resultVerifier = resultVerifier;
        }

        public Product GetProductById(int id)
        {
            return base.Repository.GetEntityByCriteria(GetByIdEvaluator(id));
        }
        private Expression<Func<Product, bool>> GetByIdEvaluator(int id) => user => user.Id.Equals(id);

        public List<Product> GetByCategory(GetProductsByCategoryModel getProductsByCategoryModel)
        {
            var category = this._categoryDomainService.GetByName(base.Mapper.Map<CategoryToGet>(getProductsByCategoryModel));

            var products = base.Repository.GetEntityListByCriteria(GetByCategoryCriteria(category));
            this._resultVerifier.IfEmptyThrowException(products.ToList());

            return products.ToList();
        }
        private Expression<Func<Product, bool>> GetByCategoryCriteria(Category category) => product => product.Categories.Contains(category);

        public void Create(NewProductModel newProductModel)
        {
            var product = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newProductModel.Name));
            this._resultVerifier.IfExistsThrowException(product, ProductType);

            this.Aggregate.Create(base.Mapper.Map<NewProduct>(newProductModel));
            base.Repository.Add(this.Aggregate);
            base.Repository.SaveChanges();
        }
        private Expression<Func<Product, bool>> GetByNameEvaluator(string productName) => product => product.Name.ToLower() == productName.ToLower();

        public void Delete(int id)
        {
            var product = GetProductById(id);
            this._resultVerifier.IfNullThrowException(product, ProductType);

            base.Repository.Delete(product);
            base.Repository.SaveChanges();
        }

        public void ModifyProperties(ModifyPropertiesProduct modifyPropertiesProduct)
        {
            var product = GetProductById(modifyPropertiesProduct.Id);
            this._resultVerifier.IfNullThrowException(product, ProductType);

            product.ModifyProperties(modifyPropertiesProduct);
            base.Repository.SaveChanges();
        }

        public void AssignBrand(AddOrRemoveProductBrandModel addOrRemoveProductBrandModel)
        {
            var product = GetProductById(addOrRemoveProductBrandModel.ProductId);
            this._resultVerifier.IfNullThrowException(product, ProductType);

            var brand = this._brandDomainService.GetById(base.Mapper.Map<GetBrandById>(addOrRemoveProductBrandModel));
            product.AssignBrand(brand);

            base.Repository.SaveChanges();
        }

        public void RemoveBrand(AddOrRemoveProductBrandModel addProductBrandModel)
        {
            var product = GetProductById(addProductBrandModel.ProductId);
            this._resultVerifier.IfNullThrowException(product, ProductType);

            var brand = this._brandDomainService.GetById(base.Mapper.Map<GetBrandById>(addProductBrandModel));
            product.RemoveBrand(brand);

            base.Repository.SaveChanges();
        }

        public void AssignCategory(AssignCategoryModel assignCategoryModel)
        {
            var product = GetProductById(assignCategoryModel.ProductId);
            this._resultVerifier.IfNullThrowException(product, ProductType);

            var category = this._categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(assignCategoryModel));
            product.AssignCategory(category);

            base.Repository.SaveChanges();
        }

        public void RemoveCategory(RemoveCategoryFromProductModel removeCategoryFromProductModel)
        {
            var product = GetProductById(removeCategoryFromProductModel.ProductId);
            this._resultVerifier.IfNullThrowException(product, ProductType);

            var category = this._categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(removeCategoryFromProductModel));
            product.RemoveCategory(category);

            base.Repository.SaveChanges();
        }

        public List<Product> OrderBy()
        {
            return base.Repository.OrderBy(OrderByNameEvaluator()).ToList();
        }
        private Expression<Func<Product, string>> OrderByNameEvaluator() => product => product.Name;
    }
}
