using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineWatchShop.Web.Models;

namespace OnlineWatchShop.Web.Contracts
{
	public interface IOrderService
	{
		IEnumerable<OrderModel> GetAll();
		OrderModel GetSingle(int id);
		Task<int> Add(OrderModel newOrder);
		Task<OrderModel> Update(OrderModel orderModel);
		Task<int> Delete(int id);
	}
}