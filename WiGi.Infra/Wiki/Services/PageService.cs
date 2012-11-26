namespace WiGi.Wiki.Services
{
	using System.Collections.Generic;
	using Exceptions;
	using Parsers;
	using Wiki;
	using Repositories;
	using Extensions;
	using System.Linq;

	public class PageService : IPageService
	{
		private static Dictionary<string, IPageParser> _parsers;

		private IPageRepository _pageRepo;

		public IContentProvider ContentProvider { get; private set; }

		static PageService()
		{
			_parsers = new Dictionary<string, IPageParser>();
		}

		public PageService(IPageRepository pageRepo, IContentProvider contentProv)
		{
			_pageRepo = pageRepo;
			ContentProvider = contentProv;
		}

		public Page GetPage(string docId)
		{
			var page = _pageRepo.Find(docId);

			if (page == null)
			{
				if (!ContentProvider.Exists(docId))
					return null;

				page = new Page
					{
						Content = ContentProvider.GetContent(docId),
						DocId = docId,
						Url = docId,
						Title = docId.Replace("-", " ").ToTitleCase()
					};

				_pageRepo.Add(page);
				_pageRepo.Save();
			}
			else if (!ContentProvider.Exists(docId))
			{
				_pageRepo.Delete(page);
				return null;
			}
			
			page.Content = ContentProvider.GetContent(docId);

			return page;
		}

		public string ToHtml(Page page)
		{
			var parser = GetParserFor(page.DocId);

			return parser.Parse(page);
		}

		public void SavePage(Page page)
		{
			_pageRepo.Attach(page);
			_pageRepo.Save();
		}

		public IPageParser GetParserFor(Page page)
		{
			return GetParserFor(page.DocId);
		}

		public IPageParser GetParserForExtension(string ext)
		{
			if (_parsers.ContainsKey(ext))
				return _parsers[ext];

			return null;
		}

		public IPageParser GetParserFor(string docId)
		{
			var ext = ContentProvider.GetFileExtension(docId);

			if (ext == null)
				return null;

			var exts = new List<string>(ext.TrimStart('.').Split('.'));

			foreach (var ex in exts)
			{
				if (!_parsers.ContainsKey("." + ex))
					throw new ParserNotFoundException("." + ex);
			}

			return exts.Count > 1 ? new PipelinePageParser(this, string.Join(".", exts)) : _parsers[ext];
		}

		public static void RegisterParser(string extension, IPageParser parser)
		{
			_parsers[extension] = parser;
		}

		public static void UnregisterParser(string extension, IPageParser parser)
		{
			_parsers.Remove(extension);
		}
	}
}
