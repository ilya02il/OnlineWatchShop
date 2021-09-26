using OnlineWatchShop.DAL.Contracts;
using OnlineWatchShop.DAL.Contracts.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

		public IQueryable<TEntity> GetAllInclude<TEntity>(params Expression<Func<TEntity, object>>[] includeExpressions)
			where TEntity : class, IEntity
		{
			var entitiesCollection = _context.Set<TEntity>().AsQueryable();

			foreach (var includeExpression in includeExpressions)
			{
				entitiesCollection = entitiesCollection.Include(includeExpression);
			}

			return entitiesCollection;
		}

		public async Task<int> Add<T>(T newEntity) where T : class, IEntity
        {
            var entity = await _context.Set<T>().AddAsync(newEntity);
            return entity.Entity.Id;
        }

        public async Task AddRange<T>(IEnumerable<T> newEntities) where T : class, IEntity
        {
            await _context.Set<T>().AddRangeAsync(newEntities);
        }

        public void Attach<T>(T entity) where T : class, IEntity
        {
            _context.Set<T>().Attach(entity);
        }

        public async Task Remove<T>(T entity) where T : class, IEntity
        {
            await Task.Run(() => _context.Set<T>().Remove(entity));
        }

        public async Task Remove<T>(Func<T, bool> selector) where T : class, IEntity
        {
	        await Task.Run(() =>
	        {
		        var entitiesRange = _context.Set<T>().Where(selector).ToList();

                if (entitiesRange.Count() == 0)
                    return;

		        _context.Set<T>().RemoveRange(entitiesRange);
	        });
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

        public async Task UpdateRange<T>(IEnumerable<T> entities) where T : class, IEntity
        {
            await Task.Run(() => _context.Set<T>().UpdateRange(entities));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
	        return _context.SaveChanges();
        }
    }
}
