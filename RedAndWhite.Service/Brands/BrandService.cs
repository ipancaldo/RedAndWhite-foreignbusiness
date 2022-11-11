﻿using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Model.Brands;
using RedAndWhite.Model.Categories;
using RedAndWhite.Repository.Brands;
using RedAndWhite.Service.Common;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Brands
{
    public class BrandService : ServiceBase<Brand, IBrandRepository>, IBrandService, IBrandDomainService
    {
        const string BrandType = "Brand";

        private readonly IResultVerifier _resultVerifier;
        private readonly ICategoryDomainService _categoryDomainService;

        public BrandService(IBrandRepository repository,
                            ICategoryDomainService categoryDomainService,
                            IResultVerifier resultVerifier,
                            IMapper mapper) 
            : base(repository, mapper)
        {
            this._resultVerifier = resultVerifier;
            this._categoryDomainService = categoryDomainService;
        }
        
        public Brand GetById(int id)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByIdEvaluator(id));
            this._resultVerifier.IfNullThrowException(brand, BrandType);

            return brand;
        }
        private Expression<Func<Brand, bool>> GetByIdEvaluator(int id) => brand => brand.Id.Equals(id);


        public Brand GetById(GetBrandById id)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByIdEvaluator(id.BrandId));
            this._resultVerifier.IfNullThrowException(brand, BrandType);

            return brand;
        }

        public Brand GetByName(NewBrand newBrand)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newBrand.BrandName));
            this._resultVerifier.IfNullThrowException(brand, BrandType);

            throw new Exception("Brand don't exist.");
        }

        //public List<Brand> GetByIds(GetBrandsById id)
        //{
        //    return base.Repository.GetEntityListByCriteria(GetByIdsExpression(id.BrandIds)).ToList();
        //}
        //private Expression<Func<Brand, bool>> GetByIdsExpression(List<int> ids) => brand => ids.Any(b => brand.Id == b);                

        public List<Brand> GetByCategory(GetCategoryByIdModel categoryModel)
        {
            var category = this._categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(categoryModel));

            var brands = base.Repository.GetEntityListByCriteria(GetByCategoryIdEvaluator(category)).ToList();
            this._resultVerifier.IfEmptyThrowException(brands);

            return brands;
        }
        private Expression<Func<Brand, bool>> GetByCategoryIdEvaluator(Category category) => brand => brand.Categories.Contains(category);

        public void Create(NewBrandModel newBrandModel)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newBrandModel.BrandName));
            this._resultVerifier.IfExistsThrowException(brand, BrandType);

            this.Aggregate.Create(base.Mapper.Map<NewBrand>(newBrandModel));
            base.Repository.Add(this.Aggregate);

            base.Repository.SaveChanges();
        }
        private Expression<Func<Brand, bool>> GetByNameEvaluator(string brandName) => brand => brand.Name.ToLower() == brandName.ToLower();

        public void Modify(ModifyPropertiesBrand modifyPropertiesBrand)
        {
            var brand = GetById(modifyPropertiesBrand.Id);
            this._resultVerifier.IfNullThrowException(brand, BrandType);

            brand.Modify(modifyPropertiesBrand);
            base.Repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var brand = GetById(id);
            this._resultVerifier.IfNullThrowException(brand, BrandType);

            base.Repository.Delete(brand);
            base.Repository.SaveChanges();
        }

        public List<Brand> OrderBy()
        {
            return base.Repository.OrderBy(OrderByNameEvaluator()).ToList();
        }
        private Expression<Func<Brand, string>> OrderByNameEvaluator() => brand => brand.Name;
    }
}
