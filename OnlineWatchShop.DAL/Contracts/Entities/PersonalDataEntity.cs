using System.ComponentModel.DataAnnotations;

namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class PersonalDataEntity : IEntity
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string Patronymic { get; set; }
		[Required]
		public string Phone { get; set; }

		public int UserId { get; set; }
		public UserEntity User { get; set; }

	}
}
