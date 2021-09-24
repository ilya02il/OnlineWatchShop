using OnlineWatchShop.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Contracts
{
	public interface IProductService
	{
		IEnumerable<ProductModel> GetAll();
		ProductModel GetSingle(int id);
		Task<int> Add(ProductModel newProduct);
		Task<ProductModel> Update(ProductModel productDto);
		Task<int> Delete(int id);
	}
}
