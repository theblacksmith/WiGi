using WiGi.UI;

[assembly: WebActivator.PreApplicationStartMethod(typeof(WiGiConfig), "Start")]

namespace WiGi.UI
{
	using WiGi.Wiki.Parsers;
	using WiGi.Wiki.Services;

	public class WiGiConfig
	{
		public static void Start()
		{
			PageService.RegisterParser(".md", MarkdownPageParser.Instance);
			PageService.RegisterParser(".html", HtmlPageParser.Instance);
			PageService.RegisterParser(".htm", HtmlPageParser.Instance);
			PageService.RegisterParser(".cshtml", RazorPageParser.Instance);
		}
	}
}