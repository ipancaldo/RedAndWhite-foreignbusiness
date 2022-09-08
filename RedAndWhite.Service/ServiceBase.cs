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

        public ServiceBase(TRepository repository)
        {
            this.Aggregate = (TDomain)Activator.CreateInstance(typeof(TDomain))!;
            this.Repository = repository;
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
