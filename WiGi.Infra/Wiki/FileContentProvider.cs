namespace WiGi.Wiki
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Linq;
	using System.Web;
	using Extensions;

	public class FileContentProvider : IContentProvider
	{
		private readonly string _baseDirectory;
		private readonly Encoding _encoding;

		public FileContentProvider(Encoding encoding = null)
		{
			_baseDirectory = HttpContext.Current.Server.MapPath(WG.Settings.RepositoryPath);
			_encoding = encoding;
		}

		public bool Exists(string docId)
		{
			var path = GetFilePath(docId);
			return path != null && File.Exists(path);
		}

		public string GetFilePath(string docId)
		{
			var baseDir = Path.GetDirectoryName(Path.Combine(_baseDirectory, docId));
			var filename = Path.GetFileNameWithoutExtension(docId);

			if(baseDir == null)
				throw new IOException("Could not find the base directory for " + Path.Combine(_baseDirectory, docId));
			
			var results = new List<string>(Directory.GetFiles(
									baseDir, 
									filename + ".*", 
									SearchOption.TopDirectoryOnly));

			if (results.Count > 1)
			{
				throw new Exception("Multiple documents found for " + docId + ": " +
				                    String.Join(", ", results) + ". <br/>" +
				                    "See " + WiGiDocs.ErrorLink("errors/multiple-docs-per-docid") + ".");
			}

			return results.FirstOrDefault();
		}

		public string GetContent(string docId)
		{
			if (!Exists(docId))
				return null;

			return File.ReadAllText(GetFilePath(docId));
		}

		public string GetFileExtension(string docId)
		{
			var path = GetFilePath(docId);

			if (path.IsNullOrWhiteSpace())
				return null;

			var filename = Path.GetFileName(path);
			var ext = "."+filename.RightOf('.');

			return ext;
		}
	}
}