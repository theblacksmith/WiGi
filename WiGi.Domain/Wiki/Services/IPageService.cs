namespace WiGi.Wiki.Services
{
	public interface IPageService
	{
		IContentProvider ContentProvider { get; }
		Page GetPage(string docId);
		void SavePage(Page page);
		IPageParser GetParserFor(Page page);
		IPageParser GetParserFor(string docId);
		IPageParser GetParserForExtension(string extension);
	}
}
