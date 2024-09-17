using Microsoft.AspNetCore.Mvc;

namespace AuthenticationManagement.API.Controllers
{
	public class CatalogsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
