using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Models;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Controllers
{
	[ApiController]
	[Route("api/[action]")]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;
		//private readonly AuthSettings _authSettings;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
			//_authSettings = authSettings;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var token = await _accountService.Login(model);

			if (token == null)
				return Unauthorized();

			return Ok(new
			{
				data = new
				{
					token
				}
			});
		}

		[HttpPost]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			await _accountService.Register(model);

			return StatusCode(204);
		}
	}
}
