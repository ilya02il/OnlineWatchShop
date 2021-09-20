using OnlineWatchShop.DAL.Contracts;
using OnlineWatchShop.DAL.Contracts.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineWatchShop.DAL.Implementations
{
	public class DbRepository : IDbRepository
    {
        private readonly DataContext _context;

        public DbRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IEntity
        {
            return _context.Set<T>().Where(selector).AsQueryable();
        }
        public IQueryable<T> GetAll<T>() where T : class, IEntity
        {
	        return _context.Set<T>().AsQueryable();
        }

        public int Add<T>(T newEntity) where T : class, IEntity
        {
            var entity = _context.Set<T>().Add(newEntity);
            return entity.Entity.Id;
        }

        public void Attach<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Attach(entity);
        }

        public async Task Remove<T>(T entity) where T : class, IEntity
        {
            await Task.Run(() => _context.Set<T>().Remove(entity));
        }

        public async Task Update<T>(T entity) where T : class, IEntity
        {
            var innerEntity = await _context.Set<T>().FindAsync(entity.Id);

            if (innerEntity == null) return;

            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.GetValue(entity) == null)
                    property.SetValue(entity, property.GetValue(innerEntity));
            }

            await Task.Run(() => _context.Entry(innerEntity).CurrentValues.SetValues(entity));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        //public IQueryable<TEntity> GetAllInclude<TEntity>(params Expression<Func<TEntity, object>>[] includeExpressions)
        //    where TEntity : class, IEntity
        //{
        //    var entitiesCollection = _context.Set<TEntity>().AsQueryable();


        //    foreach (var includeExpression in includeExpressions)
        //    {
        //        entitiesCollection = entitiesCollection.Include(includeExpression);
        //    }

        //    return entitiesCollection;
        //}
    }
}
