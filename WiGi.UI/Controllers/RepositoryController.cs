using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace WiGi.UI.Controllers
{
	using Git;

	[RoutePrefix("Repo")]
    public class RepositoryController : Controller
    {
		[GET("Update")]
        public ActionResult Update()
        {
			var ret = WiGiManager.UpdateWikiRepository();

            return View(ret);
        }

    }
}
