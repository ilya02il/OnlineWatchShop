using System.ComponentModel.DataAnnotations;

namespace OnlineWatchShop.Web.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "The login was not entered.")]
		public string Login { get; set; }
		[Required(ErrorMessage = "The password was not entered.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords are different.")]
		public string ConfirmPassword { get; set; }
	}
}
