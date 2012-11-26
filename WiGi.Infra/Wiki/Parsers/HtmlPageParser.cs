namespace WiGi.Wiki.Parsers
{
	using System;

	public class HtmlPageParser : IPageParser
	{
		protected static Lazy<IPageParser> Singleton { get; set; }

		public static IPageParser Instance
		{
			get { return Singleton.Value; }
		}

		public bool SupportsMetaInfo {
			get { return false; }
		}

		public string Parse(Page page)
		{
			return page.Content;
		}

		public dynamic GetMetaInfo(Page page)
		{
			return null;
		}

		static HtmlPageParser()
		{
			Singleton = new Lazy<IPageParser>(
				() => new HtmlPageParser(), true);
		}

		protected HtmlPageParser()
		{
		}
	}
}