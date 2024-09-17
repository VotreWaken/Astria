using Microsoft.AspNetCore.Mvc;

namespace AuthenticationManagement.API.Controllers
{
	public class OrdersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
