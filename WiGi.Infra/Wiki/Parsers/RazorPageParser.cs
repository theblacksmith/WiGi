namespace WiGi.Wiki.Parsers
{
	using System;
	using RazorEngine;
	using RazorEngine.Configuration;
	using RazorEngine.Templating;

	public class RazorPageParser : IPageParser
	{
		protected static Lazy<IPageParser> Singleton { get; set; }

		private dynamic _viewBag;

		public static IPageParser Instance
		{
			get { return Singleton.Value; }
		}

		static RazorPageParser()
		{
			Singleton = new Lazy<IPageParser>(
				() => new RazorPageParser(), true);

			var rconf = new TemplateServiceConfiguration();
			rconf.BaseTemplateType = typeof(HtmlTemplateBase<>);
			
			rconf.Namespaces.Add("WiGi");
			rconf.Namespaces.Add("WiGi.Account");
			rconf.Namespaces.Add("WiGi.Wiki");

			Razor.SetTemplateService(new TemplateService(rconf));
		}

		public bool SupportsMetaInfo {
			get { return false; }
		}

		public string Parse(Page page)
		{
			return Razor.Parse(page.Content, page);
		}

		public dynamic GetMetaInfo(Page page)
		{
			throw new NotImplementedException();
		}
	}
}
