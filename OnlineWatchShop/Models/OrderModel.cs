using System.Collections.Generic;

namespace OnlineWatchShop.Web.Models
{
	public class OrderModel
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public ICollection<ProductModel> Products { get; set; }
	}
}
