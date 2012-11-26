namespace WiGi.Wiki.Parsers
{
	using System;
	using System.Diagnostics;
	using System.Dynamic;
	using System.IO;
	using MarkdownSharp;
	using YamlDotNet.RepresentationModel;

	public class MarkdownPageParser : IPageParser
	{
		protected static Lazy<IPageParser> Singleton { get; set; }

		public static IPageParser Instance
		{
			get { return Singleton.Value; }
		}

		static MarkdownPageParser()
		{
			Singleton = new Lazy<IPageParser>(
				() => new MarkdownPageParser(), true);
			
		}

		public bool SupportsMetaInfo {
			get { return false; }
		}

		private MarkdownOptions CreateMarkdownOptions()
		{
			return new MarkdownOptions()
			{
				AutoHyperlink = true,
				AutoNewLines = false,
				EncodeProblemUrlCharacters = true,
				LinkEmails = true,
			};
		}

		private MarkdownPageParser()
		{
			_md = new Markdown(CreateMarkdownOptions());
		}

		private string _metaInfoStr;

		private string _content;

		private readonly Markdown _md;

		public string Parse(Page page)
		{
			if (page.Content == null)
				return "";
			SeparateMetaFromContent(page.Content);
			return _md.Transform(_content);
		}

		public dynamic GetMetaInfo(Page page)
		{
			throw new NotImplementedException();

			if (page.Content == null)
				return null;

			SeparateMetaFromContent(page.Content);

			if (String.IsNullOrEmpty(_metaInfoStr.Trim()))
				return null;

			// dynamic meta = new ElasticObject();
			dynamic meta = new ExpandoObject();
			// Load the stream
			var yaml = new YamlStream();
			yaml.Load(new StringReader(_metaInfoStr));

			// Examine the stream
			var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
			
			foreach (var entry in mapping.Children)
			{
				var newAtt = meta < ((YamlScalarNode)entry.Key).Value;
				newAtt <<= ((YamlScalarNode)entry.Value).Value;
				Debug.WriteLine(((YamlScalarNode)entry.Key).Value);
			}

			
			return meta;
		}

		public dynamic GetMetaInfo(Page page, dynamic ViewBag)
		{
			throw new NotImplementedException();
		}

		private void SeparateMetaFromContent(string content)
		{
			_metaInfoStr = "";
			var sr = new StringReader(content);
			var line = sr.ReadLine();

			if (line != null && line.Trim().StartsWith("---"))
			{
				line = sr.ReadLine();
				while (line != null && !line.Trim().StartsWith("---"))
				{
					_metaInfoStr += line + Environment.NewLine;
					line = sr.ReadLine();
				}
			}
			else
				_content = line;

			_content += sr.ReadToEnd();
		}
    }
}