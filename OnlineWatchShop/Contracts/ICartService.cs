using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineWatchShop.Web.Models;

namespace OnlineWatchShop.Web.Contracts
{
	public interface ICartService
	{
		IEnumerable<CartModel> GetAll();
		CartModel GetSingle(int id);
		Task<int> Add(CartModel newCart);
		Task<CartModel> Update(CartModel cartModel);
		Task<int> Delete(int id);
	}
}
