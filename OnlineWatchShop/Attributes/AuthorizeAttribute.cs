using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using OnlineWatchShop.DAL.Contracts.Entities;
using System.Collections.Generic;

namespace OnlineWatchShop.Web.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		private readonly ICollection<string> _roles;
		public AuthorizeAttribute(params string[] roles)
		{
			_roles = roles.ToList() ?? new List<string>();
		}
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			// skip authorization if action is decorated with [AllowAnonymous] attribute
			var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
			if (allowAnonymous)
				return;

			// authorization
			if (context.HttpContext.Items["User"] is not UserEntity user)
			{
				context.Result = new JsonResult(new { message = "Unauthorized" })
				{
					StatusCode = StatusCodes.Status401Unauthorized
				};
			}
			else if (_roles.Any() && !_roles.Contains(user.Role.Name))
			{
				context.Result = new JsonResult(new { message = "Forbidden" })
				{
					StatusCode = StatusCodes.Status403Forbidden
				};
			}
		}
	}
}
