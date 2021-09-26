using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineWatchShop.DAL.Contracts.Entities;

namespace OnlineWatchShop.DAL.Implementations.Configurations
{
	public class RolesConfiguration : IEntityTypeConfiguration<RoleEntity>
	{
		public void Configure(EntityTypeBuilder<RoleEntity> builder)
		{
			builder.HasData(new RoleEntity()
				{
					Id = 1,
					Name = "Admin"
				},
				new RoleEntity()
				{
					Id = 2,
					Name = "User"
				});
		}
	}
}