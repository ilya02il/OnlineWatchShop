namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class ImageEntity : IEntity
	{
		public int Id { get; set; }

		public string Url { get; set; }

		public int ProductId { get; set; }
		public ProductEntity Product { get; set; }
	}
}
