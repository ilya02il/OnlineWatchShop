using System.Collections.Generic;

namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class CartEntity : IEntity
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public UserEntity User { get; set; }

		public ICollection<CartProductEntity> CartProducts
		{
			get;
			set;
		} = new List<CartProductEntity>();
	}
}
