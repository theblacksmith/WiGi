namespace WiGi.UI.Controllers
{
	using System.Web.Mvc;

	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}
