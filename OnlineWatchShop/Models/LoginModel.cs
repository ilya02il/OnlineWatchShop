using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OnlineWatchShop.Web.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "The login was not entered.")]
		public string Login { get; set; }
		[Required(ErrorMessage = "The password was not entered.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
