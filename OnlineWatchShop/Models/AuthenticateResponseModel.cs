using System;

namespace OnlineWatchShop.Web.Models
{
	public class AuthenticateResponseModel
	{
		public string Username { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime AccessTokenExpiry { get; set; }
	}
}
