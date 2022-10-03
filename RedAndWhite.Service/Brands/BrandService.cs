using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Repository.Brands;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Brands
{
    public class BrandService : ServiceBase<Brand, IBrandRepository>, IBrandService, IBrandDomainService
    {
        public BrandService(IBrandRepository repository,
                             IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public Brand GetOrCreateByName(NewBrand newBrand)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newBrand.Name));
            if (brand != null) return brand;

            this.Aggregate.Create(newBrand);
            return this.Aggregate;
        }
        
        public Brand GetById(int id)
        {
            return base.Repository.GetEntityByCriteria(GetByIdEvaluator(id));
        }
        private Expression<Func<Brand, bool>> GetByIdEvaluator(int id) => brand => brand.Id.Equals(id);

        public Brand GetById(GetBrandById id)
        {
            return base.Repository.GetEntityByCriteria(GetByIdEvaluator(id.BrandId));
        }

        //public List<Brand> GetByIds(GetBrandsById id)
        //{
        //    return base.Repository.GetEntityListByCriteria(GetByIdsExpression(id.BrandIds)).ToList();
        //}
        //private Expression<Func<Brand, bool>> GetByIdsExpression(List<int> ids) => brand => ids.Any(b => brand.Id == b);                

        public void Create(NewBrand newBrand)
        {
            var brand = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newBrand.Name));
            if (brand is not null)
                throw new Exception("Brand already exist.");

            this.Aggregate.Create(newBrand);
            base.Repository.Add(this.Aggregate);
            base.Repository.SaveChanges();
        }
        private Expression<Func<Brand, bool>> GetByNameEvaluator(string brandName) => brand => brand.Name.ToLower() == brandName.ToLower();

        public void Modify(ModifyPropertiesBrand modifyPropertiesBrand)
        {
            var brand = GetById(modifyPropertiesBrand.Id);
            if (brand == null)
                throw new Exception("Brand don't exist.");

            brand.Modify(modifyPropertiesBrand);
            base.Repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var brand = GetById(id);
            if (brand is null)
                throw new Exception("Brand don't exist");

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
