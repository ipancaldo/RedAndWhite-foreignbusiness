using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Repository;
using System.Linq.Expressions;

namespace RedAndWhite.Service
{
    public abstract class ServiceBase<TDomain, TRepository> : IServiceBase<TDomain>
        where TDomain : BaseDomain
        where TRepository : IBaseRepository<TDomain>
    {
        protected TRepository Repository;
        protected TDomain Aggregate;
        protected readonly IMapper Mapper;

        public ServiceBase(TRepository repository,
                           IMapper mapper)
        {
            this.Aggregate = (TDomain)Activator.CreateInstance(typeof(TDomain))!;
            this.Repository = repository;
            this.Mapper = mapper;
        }

        public IEnumerable<TDomain> GetAll()
        {
            return this.Repository.GetAll();
        }

        public IEnumerable<TDomain> GetEntityListByCriteria(Expression<Func<TDomain, bool>> predicate)
        {
            return this.Repository.GetEntityListByCriteria(predicate);
        }
    }
}
