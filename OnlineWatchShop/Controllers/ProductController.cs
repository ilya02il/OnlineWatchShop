using Microsoft.AspNetCore.Mvc;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Attributes;

namespace OnlineWatchShop.Web.Controllers
{
	[Authorize]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		[Route("api/products")]
		public IActionResult GetAllProducts()
		{
			var products = _productService.GetAll();

			if (products == null)
				return BadRequest();

			return Ok(products);
		}
	}
}
