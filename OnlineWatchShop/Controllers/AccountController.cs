using System;
using Microsoft.AspNetCore.Mvc;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlineWatchShop.Web.Attributes;

namespace OnlineWatchShop.Web.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/")]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var token = await _accountService.Authenticate(model, IpAddress());

			if (token == null)
				return Unauthorized();

			SetTokenCookie(token.RefreshToken);

			return Ok(token);
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			var result = await _accountService.Register(model);

			if (result == false)
				return BadRequest();

			return StatusCode(204);
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			var response = await _accountService.RefreshToken(refreshToken, IpAddress());
			SetTokenCookie(response.RefreshToken);
			return Ok(response);
		}

		[HttpPost("logout")]
		public IActionResult RevokeToken()
		{
			var token = Request.Cookies["refreshToken"];

			if (string.IsNullOrEmpty(token))
				return BadRequest(new { message = "Token is required" });

			_accountService.RevokeToken(token, IpAddress());
			RemoveTokenCookie(token);

			return Ok(new 
				{ message = "Token revoked" }
			);
		}

		private void SetTokenCookie(string token)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTime.UtcNow.AddDays(7)
			};
			Response.Cookies.Append("refreshToken", token, cookieOptions);
		}

		private void RemoveTokenCookie(string token)
		{
			Response.Cookies.Delete("refreshToken");
		}

		private string IpAddress()
		{
			if (Request.Headers.ContainsKey("X-Forwarded-For"))
				return Request.Headers["X-Forwarded-For"];

			return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
		}
	}
}
