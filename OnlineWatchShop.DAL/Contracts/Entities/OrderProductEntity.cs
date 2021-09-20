namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class OrderProductEntity : IEntity
	{
		public int Id { get; set; }

		public int Amount { get; set; }

		public int OrderId { get; set; }
		public OrderEntity Order { get; set; }

		public int ProductId { get; set; }
		public ProductEntity Product { get; set; }
	}
}
