using System.Threading.Tasks;
using OnlineWatchShop.Web.Models;

namespace OnlineWatchShop.Web.Contracts
{
	public interface IAccountService
	{
		Task<string> Login(LoginModel model);
		Task<bool> Register(RegisterModel model);
		Task Authenticate(string login);
	}
}
