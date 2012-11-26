namespace WiGi.UI.Models
{
	using System.Web.WebPages;

	public class PageListOptions
	{
		public bool ShowDirectories { get; set; }
		public bool ShowPages { get; set; }
		public string Title { get; set; }

		/// <summary>
		/// A directory path related to the repository root
		/// </summary>
		public string Directory { get; set; }

		/// <summary>
		/// Shows a list of directories and pages under the specified directory
		/// </summary>
		/// <param name="dir">A directory path related to the repository root</param>
		public PageListOptions(string dir = "")
		{
			Title = "Page List";
			ShowDirectories = true;
			ShowPages = true;

			Directory = dir;
		}
	}
}