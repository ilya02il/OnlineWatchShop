using Microsoft.AspNetCore.Mvc;
using OnlineWatchShop.Web.Attributes;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Models;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Controllers
{
	[Authorize("Admin")]
	[ApiController]
	[Route("api/")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("users")]
		public IActionResult GetAllUsers()
		{
			var users = _userService.GetAll();

			if (users == null)
				return BadRequest();

			return Ok(users);
		}

		[HttpGet("users/{id:int}")]
		public IActionResult GetSingleUser(int id)
		{
			var user = _userService.GetSingle(id);

			if (user == null)
				return BadRequest();

			return Ok(user);
		}

		[HttpPost("users")]
		public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
		{
			if (userModel == null)
				return BadRequest();

			int newId = await _userService.Add(userModel);

			return Ok(newId);
		}

		[HttpPut("users/{id:int}")]
		public async Task<IActionResult> UpdateUser(int id, [FromBody] UserModel userModel)
		{
			if (userModel == null)
				return BadRequest();

			userModel.Id = id;
			var updatedModel = await _userService.Update(userModel);

			return Ok(updatedModel);
		}

		[HttpDelete("users/{id:int}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var deletedId = await _userService.Delete(id);

			if (deletedId == 0 || deletedId != id)
				BadRequest();

			return Ok(deletedId);
		}
	}
}
