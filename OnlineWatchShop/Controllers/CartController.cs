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
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;

		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}

		[HttpGet("carts")]
		public IActionResult GetAllCarts()
		{
			var carts = _cartService.GetAll();

			if (carts == null)
				return BadRequest();

			return Ok(carts);
		}

		[HttpGet("carts/{id:int}")]
		public IActionResult GetSingleCart(int id)
		{
			var cart = _cartService.GetSingle(id);

			if (cart == null)
				return BadRequest();

			return Ok(cart);
		}

		[HttpPost("carts")]
		public async Task<IActionResult> CreateCart([FromBody] CartModel cartModel)
		{
			if (cartModel == null)
				return BadRequest();

			int newId = await _cartService.Add(cartModel);

			return Ok(newId);
		}

		[HttpPut("carts/{id:int}")]
		public async Task<IActionResult> UpdateCart(int id, [FromBody] CartModel cartModel)
		{
			if (cartModel == null)
				return BadRequest();

			cartModel.Id = id;
			var updatedModel = await _cartService.Update(cartModel);

			return Ok(updatedModel);
		}

		[HttpDelete("carts/{id:int}")]
		public async Task<IActionResult> DeleteCart(int id)
		{
			var deletedId = await _cartService.Delete(id);

			if (deletedId == 0 || deletedId != id)
				BadRequest();

			return Ok(deletedId);
		}
	}
}
