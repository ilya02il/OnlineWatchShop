﻿using System.Collections.Generic;

namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class ProductEntity : IEntity
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Brand { get; set; }
		public string Country { get; set; }
		public string Mechanism { get; set; }
		public string Material { get; set; }
		public int Warranty { get; set; }
		public string TargetGender { get; set; }

		public StoredProductEntity StoredProduct { get; set; }

		public ICollection<OrderProductEntity> OrderProduct
		{
			get;
			set;
		} = new List<OrderProductEntity>();
	}
}
