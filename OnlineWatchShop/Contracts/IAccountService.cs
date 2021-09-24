using OnlineWatchShop.Web.Models;
using System.Threading.Tasks;
using OnlineWatchShop.DAL.Contracts.Entities;

namespace OnlineWatchShop.Web.Contracts
{
	public interface IAccountService
	{
		Task<AuthenticateResponseModel> Authenticate(LoginModel model, string ipAddress);
		Task<bool> Register(RegisterModel model);
		Task<AuthenticateResponseModel> RefreshToken(string token, string ipAddress);
		void RevokeToken(string token, string ipAddress);
		UserEntity GetById(int id);
	}
}
