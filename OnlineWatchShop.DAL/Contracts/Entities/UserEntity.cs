namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class UserEntity : IEntity
	{
		public int Id { get; set; }

		public string Login { get; set; }
		public string HashedPassword { get; set; }
		public string Salt { get; set; }

		public int RoleId { get; set; }
		public RoleEntity Role { get; set; }
	}
}
