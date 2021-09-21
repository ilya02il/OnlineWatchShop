using OnlineWatchShop.DAL.Contracts.Entities;
using System.Collections.Generic;

namespace OnlineWatchShop.Web.Models
{
	public class ProductDto
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Brand { get; set; }
		public string Country { get; set; }
		public string Mechanism { get; set; }
		public string Material { get; set; }
		public int Warranty { get; set; }
		public string TargetGender { get; set; }

		public ICollection<OrderProductEntity> OrderProduct { get; set; }
	}
}
