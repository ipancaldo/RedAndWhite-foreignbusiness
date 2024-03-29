﻿using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Infrastructure.Enums;
using RedAndWhite.Model.Products;
using RedAndWhite.Model.Shared;
using RedAndWhite.Repository.Products;
using RedAndWhite.Service.Common;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Products
{
    public class ProductService : ServiceBase<Product, IProductRepository>, IProductService
    {
        private readonly IBrandDomainService _brandDomainService;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IResultVerifierService _resultVerifier;

        const string ProductType = "Product";

        public ProductService(IProductRepository repository,
                              IBrandDomainService brandDomainService,
                              ICategoryDomainService categoryDomainService,
                              IResultVerifierService resultVerifier,
                              IMapper mapper)
            : base(repository, mapper)
        {
            _brandDomainService = brandDomainService;
            _categoryDomainService = categoryDomainService;
            _resultVerifier = resultVerifier;
        }

        public List<ProductModel> GetAllProducts()
        {
            return base.Mapper.Map<List<ProductModel>>(base.Repository.GetAll().ToList());
        }

        public async Task<Product> GetById(int id)
        {
            return await base.Repository.GetEntityByCriteria(GetByIdEvaluator(id));
        }
        private Expression<Func<Product, bool>> GetByIdEvaluator(int id) => user => user.Id.Equals(id);

        public async Task<List<ProductModel>> GetByCategoryId(GetProductByIdModel getProductByIdModel)
        {
            var category = _categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(getProductByIdModel));

            var products = await base.Repository.GetEntityListByCriteria(GetByCategoryCriteria(category));

            return base.Mapper.Map<List<ProductModel>>(products.ToList());
        }
        private Expression<Func<Product, bool>> GetByCategoryCriteria(Category category) => product => product.Categories.Contains(category);


        // GetByBrands => put it on the IService
        public async Task<List<ProductModel>> GetByBrand(GetProductsByBrandModel getProductsByBrandModel)
        {
            var brand = _brandDomainService.GetByName(base.Mapper.Map<NewBrand>(getProductsByBrandModel));

            var products = await base.Repository.GetEntityListByCriteria(GetByBrandCriteria(brand));

            return base.Mapper.Map<List<ProductModel>>(products.ToList());
        }

        private Expression<Func<Product, bool>> GetByBrandCriteria(Brand brand) => product => product.Brands.Contains(brand);

        //Implement Product by brand as it is done in GetByCategory
        //Implement the interface
        //Create a Category and assign a product to that Category
        //Create a test:
        //Class name = GetProductTest


        public async Task<ResultDTO<Product>> Create(NewProductModel newProductModel)
        {
            var product = await base.Repository.GetEntityByCriteria(GetByNameEvaluator(newProductModel.Name));

            var result = _resultVerifier.IfExistsReturnFailed(product);
            if (result.ResultStatus == ResultStatusEnum.Failed)
                return result;

            base.Aggregate.Create(base.Mapper.Map<NewProduct>(newProductModel));
            await base.Repository.Add(base.Aggregate);
            await base.Repository.SaveChanges();

            return result;
        }
        private Expression<Func<Product, bool>> GetByNameEvaluator(string productName) => product => product.Name.ToLower() == productName.ToLower();

        public async Task Delete(int id)
        {
            var product = await GetById(id);
            _resultVerifier.IfNullThrowException(product, ProductType);

            base.Repository.Delete(product);
            await base.Repository.SaveChanges();
        }

        public async Task Update(ModifyPropertiesProduct modifyPropertiesProduct)
        {
            var product = await GetById(modifyPropertiesProduct.Id);
            _resultVerifier.IfNullThrowException(product, ProductType);

            base.Aggregate = product;
            base.Aggregate.ModifyProperties(modifyPropertiesProduct);
            await base.Repository.SaveChanges();
        }

        public async Task AssignBrand(AddOrRemoveProductBrandModel addOrRemoveProductBrandModel)
        {
            var product = await GetById(addOrRemoveProductBrandModel.ProductId);
            _resultVerifier.IfNullThrowException(product, ProductType);

            base.Aggregate = product;

            var brand = _brandDomainService.GetById(base.Mapper.Map<GetBrandById>(addOrRemoveProductBrandModel));
            base.Aggregate.AssignBrand(brand);

            await base.Repository.SaveChanges();
        }

        public async Task RemoveBrand(AddOrRemoveProductBrandModel addProductBrandModel)
        {
            var product = await GetById(addProductBrandModel.ProductId);
            _resultVerifier.IfNullThrowException(product, ProductType);

            base.Aggregate = product;

            var brand = _brandDomainService.GetById(base.Mapper.Map<GetBrandById>(addProductBrandModel));
            base.Aggregate.RemoveBrand(brand);

            await base.Repository.SaveChanges();
        }

        public async Task AssignCategory(AssignCategoryModel assignCategoryModel)
        {
            var product = await GetById(assignCategoryModel.ProductId);
            _resultVerifier.IfNullThrowException(product, ProductType);

            base.Aggregate = product;

            var category = _categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(assignCategoryModel));
            base.Aggregate.AssignCategory(category);

            await base.Repository.SaveChanges();
        }

        public async Task RemoveCategory(RemoveCategoryFromProductModel removeCategoryFromProductModel)
        {
            var product = await GetById(removeCategoryFromProductModel.ProductId);
            _resultVerifier.IfNullThrowException(product, ProductType);

            base.Aggregate = product;

            var category = _categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(removeCategoryFromProductModel));
            base.Aggregate.RemoveCategory(category);

            await base.Repository.SaveChanges();
        }

        public List<Product> OrderBy()
        {
            return base.Repository.OrderBy(OrderByNameEvaluator()).ToList();
        }
        private Expression<Func<Product, string>> OrderByNameEvaluator() => product => product.Name;
    }
}