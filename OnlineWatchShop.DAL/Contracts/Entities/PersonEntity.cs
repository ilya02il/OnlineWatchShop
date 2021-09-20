using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class PersonEntity : IEntity
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string Patronymic { get; set; }
		[Required]
		public DateTime BirthDate { get; set; }
		[Required]
		public string Phone { get; set; }
		public string SecondPhone { get; set; }

		public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
	}
}
