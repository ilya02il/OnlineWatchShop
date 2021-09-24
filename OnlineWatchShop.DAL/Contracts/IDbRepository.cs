using OnlineWatchShop.DAL.Contracts.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineWatchShop.DAL.Contracts
{
	public interface IDbRepository
	{
		IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IEntity;
		IQueryable<T> GetAll<T>() where T : class, IEntity;
		IQueryable<T> GetAllInclude<T>(params Expression<Func<T, object>>[] includeExpressions)
			where T : class, IEntity;

		int Add<T>(T newEntity) where T : class, IEntity;
		//Task AddRange<T>(IEnumerable<T> newEntities) where T : class, IEntity;

		//Task Delete<T>(int entityId) where T : class, IEntity;

		Task Remove<T>(T entity) where T : class, IEntity;
		Task Remove<T>(Func<T, bool> selector) where T : class, IEntity;
		//Task RemoveRange<T>(IEnumerable<T> entities) where T : class, IEntity;

		Task Update<T>(T entity) where T : class, IEntity;
		//Task UpdateRange<T>(IEnumerable<T> entities) where T : class, IEntity;

		Task<int> SaveChangesAsync();
		int SaveChanges();
	}
}
