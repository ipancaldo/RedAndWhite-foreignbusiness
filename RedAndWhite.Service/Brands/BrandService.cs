﻿using AutoMapper;
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

        public Brand GetOrCreateBrandByName(NewBrand newBrand)
        {
            var brand = base.Repository.GetEntityByCriteria(NameEvaluator(newBrand.Name));
            if (brand != null) return brand;

            this.Aggregate.Create(newBrand);
            return this.Aggregate;
        }
        private Expression<Func<Brand, bool>> NameEvaluator(string a) => b => b.Name == a;

        public Brand GetBrandById(int id)
        {
            return base.Repository.GetEntityByCriteria(GetByIdExpression(id));
        }
        private Expression<Func<Brand, bool>> GetByIdExpression(int id) => brand => brand.Id.Equals(id);

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

        public void Modify()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var brand = GetBrandById(id);
            if (brand is null)
                throw new Exception("Brand doesn't exist");

            base.Repository.Delete(brand);
            base.Repository.SaveChanges();
        }
    }
}
