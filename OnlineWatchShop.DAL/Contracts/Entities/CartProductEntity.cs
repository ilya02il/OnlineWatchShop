namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class CartProductEntity : IEntity
	{
		public int Id { get; set; }

		public int Amount { get; set; }

		public int CartId { get; set; }
		public CartEntity Cart { get; set; }

		public int ProductId { get; set; }
		public ProductEntity Product { get; set; }
	}
}
