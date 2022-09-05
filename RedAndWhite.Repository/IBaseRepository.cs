using System.Linq.Expressions;

namespace RedAndWhite.Repository
{
    public interface IBaseRepository<TDomain>
    {
        IEnumerable<TDomain> GetAll();
        void Add(TDomain entity);
        TDomain GetEntityByCriteria(Expression<Func<TDomain, bool>> predicate);
        void SaveChanges();
    }
}
