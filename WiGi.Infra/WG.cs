namespace WiGi
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web;
	using Ninject;
	using Wiki;
	using Wiki.Repositories;
	using Extensions;

	// todo: think of a better name for this. Using WiGi is NOT a good idea
	public class WG
	{
		private static IKernel Kernel;

		private static WiGiSettings _settings;
		public static WiGiSettings Settings {
			get
			{
				if (_settings == null)
					_settings = Kernel.Get<WiGiSettings>();

				return _settings;
			}
		}

		public WG(IKernel kernel)
		{
			Kernel = kernel;
		}

		/// <summary>
		/// Returns a list of files for the pages in the root of the passed directory
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static List<string> ListFiles(string dir)
		{
			var path = Path.Combine(HttpContext.Current.Server.MapPath(Settings.RepositoryPath), dir);

			return (from file in Directory.EnumerateFiles(path)
					select Path.GetFileName(file)).ToList();
		}

		/// <summary>
		/// Returns a list of doc id's for the pages in the root of the passed directory
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static List<DocId> ListPages(string dir)
		{
			var repoDir = HttpContext.Current.Server.MapPath(Settings.RepositoryPath);
			var path = Path.Combine(repoDir, dir);

			return (from file in Directory.EnumerateFiles(path)
					where !Path.GetFileName(file).StartsWith(".")
					select DocId.ForPath(file.Replace(repoDir, ""))).ToList();
		}

		public static List<string> ListDirectories(string dir)
		{
			var path = Path.Combine(HttpContext.Current.Server.MapPath(Settings.RepositoryPath), dir);

			var dirs = Directory.EnumerateDirectories(path).Where(d => !Path.GetFileName(d).StartsWith("."));

			return (from file in dirs select Path.GetFileName(file)).ToList();
		}
	}
}
