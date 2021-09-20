using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineWatchShop.DAL.Contracts.Entities;
using OnlineWatchShop.DAL.Implementations.Configurations;

namespace OnlineWatchShop.DAL.Implementations
{
	public class DataContext : DbContext
	{
		public DbSet<UserEntity> Users { get; set; }
		public DbSet<RoleEntity> Roles { get; set; }
		public DbSet<ProductEntity> Products { get; set; }
		public DbSet<StoredProductEntity> StoredProducts { get; set; }
		public DbSet<PersonEntity> Persons { get; set; }
		public DbSet<OrderEntity> Orders { get; set; }
		public DbSet<OrderProductEntity> OrderProducts { get; set; }

		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new RolesConfiguration());
		}

		public async Task<int> SaveChangesAsync()
		{
			return await base.SaveChangesAsync();
		}

		public DbSet<T> DbSet<T>() where T : class
		{
			return Set<T>();
		}
	}
}
