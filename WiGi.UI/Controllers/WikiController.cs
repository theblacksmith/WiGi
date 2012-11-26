namespace WiGi.UI.Controllers
{
	using System.Dynamic;
	using System.IO;
	using System.Web.Mvc;
	using System.Web.WebPages;
	using AttributeRouting;
	using AttributeRouting.Web.Mvc;
	using AutoMapper;
	using Models;
	using Wiki.Services;

	[RoutePrefix("")]
	[HandleError]
    public class WikiController : Controller
	{
		private readonly IPageService _pageServ;

		static WikiController()
		{
			PageViewModel.Map();
		}

		public WikiController(IPageService pageServ)
		{
			_pageServ = pageServ;
		}

		[GET("{docId?}")]
        //[OutputCache(CacheProfile = "KiwiWikiCache")]
        public ActionResult Page(string docId)
		{
			if (docId.IsEmpty())
				docId = "home";

			var page = _pageServ.GetPage(docId);

			if (page == null)
			{
				var dir = Path.Combine(Server.MapPath(WG.Settings.RepositoryPath), docId);

				if (Directory.Exists(dir))
				{
					page = _pageServ.GetPage(docId.TrimEnd('/') + "/home");

					if(page == null)
						return View("Directory", new DirectoryViewModel()
							{
								Name = Path.GetFileName(dir),
								DocId = docId
							});
				}
				else
					return HttpNotFound();
			}
			
			var parser = _pageServ.GetParserFor(page);

			var model = Mapper.Map<Wiki.Page, PageViewModel>(page);

			// todo: remove this. After AutoMapper decides to map it!
			model.About = page.About;

			model.Html = parser.Parse(page);

            return View(model);
        }
    }
}
