using Microsoft.EntityFrameworkCore;
using RedAndWhite.Data;
using RedAndWhite.Domain;
using System.Linq.Expressions;

namespace RedAndWhite.Repository
{
    public class BaseRepository<TDomain> : IBaseRepository<TDomain>
        where TDomain : BaseDomain
    {
        protected readonly RedAndWhiteContext _redAndWhiteContext;

        protected DbSet<TDomain> DbSet { get; }

        public BaseRepository(RedAndWhiteContext redAndWhiteContext)
        {
            _redAndWhiteContext = redAndWhiteContext;
            DbSet = redAndWhiteContext.Set<TDomain>();
        }

        public IEnumerable<TDomain> GetAll() => DbSet.AsEnumerable<TDomain>();

        public TDomain GetEntityByCriteria(Expression<Func<TDomain, bool>> predicate) => DbSet.FirstOrDefault(predicate)!;

        public IEnumerable<TDomain> GetEntityListByCriteria(Expression<Func<TDomain, bool>> predicate) => DbSet.Where(predicate).ToList();
        
        public async Task Add(TDomain entity) => await _redAndWhiteContext.AddAsync(entity);

        public void Delete(TDomain entity) => DbSet.Remove(entity);

        public IEnumerable<TDomain> OrderBy(Expression<Func<TDomain, string>> predicate) => (IEnumerable<TDomain>)DbSet.OrderBy(predicate);

        public IEnumerable<TDomain> OrderByDescending(Expression<Func<TDomain, string>> predicate) => (IEnumerable<TDomain>)DbSet.OrderByDescending(predicate);

        public async Task SaveChanges() => await _redAndWhiteContext.SaveChangesAsync();
    }
}
