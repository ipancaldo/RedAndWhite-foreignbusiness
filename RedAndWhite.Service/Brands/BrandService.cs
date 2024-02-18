using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Enums;
using RedAndWhite.Model.Brands;
using RedAndWhite.Model.Categories;
using RedAndWhite.Model.Shared;
using RedAndWhite.Repository.Brands;
using RedAndWhite.Service.Common;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Brands
{
    public class BrandService : ServiceBase<Brand, IBrandRepository>, IBrandService, IBrandDomainService
    {
        const string BrandType = "Brand";

        private readonly IResultVerifierService _resultVerifier;
        private readonly ICategoryDomainService _categoryDomainService;

        public BrandService(IBrandRepository repository,
                            ICategoryDomainService categoryDomainService,
                            IResultVerifierService resultVerifier,
                            IMapper mapper)
            : base(repository, mapper)
        {
            _resultVerifier = resultVerifier;
            _categoryDomainService = categoryDomainService;
        }

        public List<BrandModel> GetAllBrands()
        {
            return base.Mapper.Map<List<BrandModel>>(base.Repository.GetAll().ToList());
        }

        public Brand GetById(int id)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByIdEvaluator(id));
            _resultVerifier.IfNullThrowException(brand, BrandType);

            return brand;
        }
        private Expression<Func<Brand, bool>> GetByIdEvaluator(int id) => brand => brand.Id.Equals(id);

        public Brand GetById(GetBrandById id)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByIdEvaluator(id.BrandId));
            _resultVerifier.IfNullThrowException(brand, BrandType);

            return brand;
        }

        public Brand GetByName(NewBrand newBrand)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newBrand.BrandName));
            _resultVerifier.IfNullThrowException(brand, BrandType); //TODO: CORRECT

            throw new Exception("Brand don't exist."); //TODO: CORRECT
        }

        //public List<Brand> GetByIds(GetBrandsById id)
        //{
        //    return base.Repository.GetEntityListByCriteria(GetByIdsExpression(id.BrandIds)).ToList();
        //}
        //private Expression<Func<Brand, bool>> GetByIdsExpression(List<int> ids) => brand => ids.Any(b => brand.Id == b);                

        public async Task<List<Brand>> GetByCategory(GetCategoryByIdModel categoryModel)
        {
            var category = _categoryDomainService.GetById(base.Mapper.Map<GetCategoryById>(categoryModel));

            return (await base.Repository.GetEntityListByCriteria(GetByCategoryIdEvaluator(category))).ToList();
        }
        private Expression<Func<Brand, bool>> GetByCategoryIdEvaluator(Category category) => brand => brand.Categories.Contains(category);

        public async Task<ResultDTO<Brand>> Create(NewBrandModel newBrandModel)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newBrandModel.BrandName));

            var resultDTO = _resultVerifier.IfExistsReturnFailed(brand);
            if (resultDTO.ResultStatus == ResultStatusEnum.Failed)
                return resultDTO;

            base.Aggregate.Create(base.Mapper.Map<NewBrand>(newBrandModel));
            await base.Repository.Add(base.Aggregate);
            await base.Repository.SaveChanges();

            return resultDTO;
        }
        private Expression<Func<Brand, bool>> GetByNameEvaluator(string brandName) => brand => brand.Name.ToLower() == brandName.ToLower();

        public async Task Update(ModifyPropertiesBrand modifyPropertiesBrand)
        {
            var brand = GetById(modifyPropertiesBrand.Id);
            _resultVerifier.IfNullThrowException(brand, BrandType);

            base.Aggregate = brand;
            base.Aggregate.Modify(modifyPropertiesBrand);
            await base.Repository.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var brand = GetById(id);
            _resultVerifier.IfNullThrowException(brand, BrandType);

            base.Repository.Delete(brand);
            await base.Repository.SaveChanges();
        }

        public List<Brand> OrderBy()
        {
            return base.Repository.OrderBy(OrderByNameEvaluator()).ToList();
        }
        private Expression<Func<Brand, string>> OrderByNameEvaluator() => brand => brand.Name;
    }
}