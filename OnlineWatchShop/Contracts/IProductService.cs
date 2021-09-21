using OnlineWatchShop.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Contracts
{
	public interface IProductService
	{
		IEnumerable<ProductDto> GetAll();
		ProductDto GetSingle(int id);
		Task<int> Add(ProductDto newProduct);
		Task<ProductDto> Update(ProductDto productDto);
		Task<int> Delete(int id);
	}
}
