using System.Collections.Generic;

namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class OrderEntity : IEntity
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public UserEntity User { get; set; }

		public ICollection<OrderProductEntity> OrderProducts
		{
			get;
			set;
		} = new List<OrderProductEntity>();
	}
}
