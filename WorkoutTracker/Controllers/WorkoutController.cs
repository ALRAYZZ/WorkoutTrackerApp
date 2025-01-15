using Microsoft.AspNetCore.Mvc;

namespace WorkoutTracker.Controllers
{
	public class WorkoutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
