using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineWatchShop.Web.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[action]")]
	public class ProductController : ControllerBase
	{
		
	}
}
