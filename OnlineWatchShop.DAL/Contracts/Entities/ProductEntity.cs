using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

		public ICollection<OrderProductEntity> OrderProducts
		{
			get;
			set;
		} = new List<OrderProductEntity>();

		[InverseProperty("Product")]
		public virtual ICollection<CartProductEntity> CartProducts
		{
			get;
			set;
		} = new List<CartProductEntity>();
		public ICollection<ImageEntity> Images
		{
			get;
			set;
		} = new List<ImageEntity>();
	}
}
