using System.Linq.Expressions;

namespace RedAndWhite.Repository
{
    public interface IBaseRepository<TDomain>
    {
        IEnumerable<TDomain> GetAll();
        TDomain GetEntityByCriteria(Expression<Func<TDomain, bool>> predicate);
        IEnumerable<TDomain> GetEntityListByCriteria(Expression<Func<TDomain, bool>> predicate);
        void Add(TDomain entity);
        void Delete(TDomain entity);
        IEnumerable<TDomain> OrderBy(Expression<Func<TDomain, string>> predicate);
        IEnumerable<TDomain> OrderByDescending(Expression<Func<TDomain, string>> predicate);
        void SaveChanges();
    }
}
