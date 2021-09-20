using System.Collections.Generic;

namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class RoleEntity : IEntity
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
	}
}
