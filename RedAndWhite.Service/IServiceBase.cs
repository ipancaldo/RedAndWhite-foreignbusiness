using System.Linq.Expressions;

namespace RedAndWhite.Service
{
    public interface IServiceBase<TDomain>
    {
        IEnumerable<TDomain> GetAll();

        IEnumerable<TDomain> GetEntityListByCriteria(Expression<Func<TDomain, bool>> predicate);
    }
}
