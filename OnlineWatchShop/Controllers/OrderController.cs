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
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet("orders")]
		public IActionResult GetAllOrders()
		{
			var orders = _orderService.GetAll();

			if (orders == null)
				return BadRequest();

			return Ok(orders);
		}

		[HttpGet("orders/{id:int}")]
		public IActionResult GetSingleOrder(int id)
		{
			var order = _orderService.GetSingle(id);

			if (order == null)
				return BadRequest();

			return Ok(order);
		}

		[HttpPost("orders")]
		public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
		{
			if (orderModel == null)
				return BadRequest();

			int newId = await _orderService.Add(orderModel);

			return Ok(newId);
		}

		[HttpPut("orders/{id:int}")]
		public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderModel orderModel)
		{
			if (orderModel == null)
				return BadRequest();

			orderModel.Id = id;
			var updatedModel = await _orderService.Update(orderModel);

			return Ok(updatedModel);
		}

		[HttpDelete("orders/{id:int}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var deletedId = await _orderService.Delete(id);

			if (deletedId == 0 || deletedId != id)
				BadRequest();

			return Ok(deletedId);
		}
	}
}
