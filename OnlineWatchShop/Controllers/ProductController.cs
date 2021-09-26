using Microsoft.AspNetCore.Mvc;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Attributes;
using OnlineWatchShop.Web.Models;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("products")]
		public IActionResult GetAllProducts()
		{
			var products = _productService.GetAll();

			if (products == null)
				return BadRequest();

			return Ok(products);
		}

		[HttpGet("products/{id:int}")]
		public IActionResult GetSingleProduct(int id)
		{
			var product = _productService.GetSingle(id);

			if (product == null)
				return BadRequest();

			return Ok(product);
		}

		[HttpPost("products")]
		public async Task<IActionResult> CreateProduct([FromBody]ProductModel productModel)
		{
			if (productModel == null)
				return BadRequest();

			int newId = await _productService.Add(productModel);

			return Ok(newId);
		}

		[HttpPut("products/{id:int}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromBody]ProductModel productModel)
		{
			if (productModel == null)
				return BadRequest();

			productModel.Id = id;
			var updatedModel = await _productService.Update(productModel);

			return Ok(updatedModel);
		}

		[HttpDelete("products/{id:int}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var deletedId = await _productService.Delete(id);

			if (deletedId == 0 || deletedId != id)
				BadRequest();

			return Ok(deletedId);
		}
	}
}
