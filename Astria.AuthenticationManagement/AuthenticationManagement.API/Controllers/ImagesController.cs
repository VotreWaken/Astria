using Microsoft.AspNetCore.Mvc;

namespace AuthenticationManagement.API.Controllers
{
	public class ImagesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
