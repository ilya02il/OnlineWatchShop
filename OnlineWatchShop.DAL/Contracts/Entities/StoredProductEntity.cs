namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class StoredProductEntity : IEntity
	{
		public int Id { get; set; }

		public int Amount { get; set; }

		public int ProductId { get; set; }
		public ProductEntity Product { get; set; }
	}
}
