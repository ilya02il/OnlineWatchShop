using OnlineWatchShop.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace OnlineWatchShop.Web.Contracts
{
	public interface IUserService
	{
		IEnumerable<UserModel> GetAll();
		UserModel GetSingle(int id);
		Task<int> Add(UserModel newUser);
		Task<UserModel> Update(UserModel productModel);
		Task<int> Delete(int id);
	}
}
