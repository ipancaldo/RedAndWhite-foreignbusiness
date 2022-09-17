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
            this._redAndWhiteContext = redAndWhiteContext;
            this.DbSet = redAndWhiteContext.Set<TDomain>();
        }

        public IEnumerable<TDomain> GetAll() => this.DbSet.AsEnumerable<TDomain>();

        public IEnumerable<TDomain> GetEntityListByCriteria(Expression<Func<TDomain, bool>> predicate) => (IEnumerable<TDomain>)this.DbSet.Where(predicate);
        
        public void Add(TDomain entity) => this._redAndWhiteContext.Add(entity);

        public void Delete(TDomain entity) => this.DbSet.Remove(entity);

        public TDomain GetEntityByCriteria(Expression<Func<TDomain, bool>> predicate) => this.DbSet.FirstOrDefault(predicate);

        public void SaveChanges() => this._redAndWhiteContext.SaveChanges();
    }
}
