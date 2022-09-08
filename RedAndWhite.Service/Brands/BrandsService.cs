﻿using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Repository.Brands;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Brands
{
    public class BrandsService : ServiceBase<Brand, IBrandsRepository>, IBrandsService, IBrandDomainService
    {
        public BrandsService(IBrandsRepository repository) 
            : base(repository)
        {
        }

        public Brand GetOrCreateBrandByName(string brandName)
        {
            var brand = base.Repository.GetEntityByCriteria(NameEvaluator(brandName));
            //Implement the Null pattern design
            if (brand != null) return brand;

            this.Aggregate.Create(brandName);
            return this.Aggregate;
        }
        private Expression<Func<Brand, bool>> NameEvaluator(string a) => b => b.Name == a;
    }
}
