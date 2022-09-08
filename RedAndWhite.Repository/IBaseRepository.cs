using System.Linq.Expressions;

namespace RedAndWhite.Repository
{
    public interface IBaseRepository<TDomain>
    {
        IEnumerable<TDomain> GetAll();
        IEnumerable<TDomain> GetEntityListByCriteria(Expression<Func<TDomain, bool>> predicate);
        void Add(TDomain entity);
        TDomain GetEntityByCriteria(Expression<Func<TDomain, bool>> predicate);
        void SaveChanges();
    }
}
