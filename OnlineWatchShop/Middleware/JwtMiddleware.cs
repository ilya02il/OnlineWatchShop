﻿using Microsoft.AspNetCore.Http;
using OnlineWatchShop.Web.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Middleware
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;

		public JwtMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, IAccountService accountService, IJwtUtils jwtUtils)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			var userId = jwtUtils.ValidateJwtToken(token);
			if (userId != null)
			{
				// attach user to context on successful jwt validation
				context.Items["User"] = accountService.GetById(userId.Value);
			}

			await _next(context);
		}
	}
}
