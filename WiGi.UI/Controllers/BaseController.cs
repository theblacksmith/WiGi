namespace WiGi.UI.Controllers
{
	using System.Web.Mvc;

	public class BaseController : Controller
	{
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (RouteData.Values["controller"].ToString().ToLower() != "install")
			{
				var installer = new Installer(Server.MapPath("~"));

				if (!installer.IsInstalled())
					filterContext.Result = RedirectToAction("Index", "Install");
			}
		}
	}
}