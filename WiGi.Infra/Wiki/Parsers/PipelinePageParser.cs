namespace WiGi.Wiki.Parsers
{
	using System.Collections.Generic;
	using System.Linq;
	using Exceptions;
	using Services;
	using Extensions;


	public class PipelinePageParser : IPageParser
	{
		private IPageService _pageService;

		private readonly Stack<IPageParser> _parsers;

		public PipelinePageParser(IPageService pageService, string extensionPipe)
		{
			_pageService = pageService;
			_parsers = BuildParserStack(extensionPipe);
		}

		public bool SupportsMetaInfo {
			get
			{
				if (_parsers.Any(p => p.SupportsMetaInfo))
					return true;

				return false;
			}
		}

		public string Parse(Page page)
		{
			var contentBackup = page.Content;
			var parsers = _parsers.Clone();

			while (parsers.Count > 0)
			{
				var parser = parsers.Pop();
				page.Content = parser.Parse(page);
			}

			var parsedContent = page.Content;
			page.Content = contentBackup;

			return parsedContent;
		}

		private Stack<IPageParser> BuildParserStack(string extensionPipe)
		{
			var parsers = new Stack<IPageParser>();
			var exts = new Queue<string>(extensionPipe.TrimStart('.').Split('.'));

			while (exts.Count > 0)
			{
				extensionPipe = exts.Dequeue();
				var parser = _pageService.GetParserForExtension("." + extensionPipe);

				if (parser == null)
					throw new ParserNotFoundException("." + extensionPipe);

				parsers.Push(parser);
			}

			return parsers;
		}

		public dynamic GetMetaInfo(Page page)
		{
			var parsers = _parsers.Clone();

			while (parsers.Count > 0)
			{
				var parser = parsers.Pop();
				if (parser.SupportsMetaInfo)
					return parser.GetMetaInfo(page);
			}

			return null;
		}
	}
}
