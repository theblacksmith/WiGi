namespace WiGi.Wiki
{
	using System.Collections.Generic;

	public class Page
	{
		public int Id { get; set; }

		/// <summary>
		/// The title of this page
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// A brief description about this page
		/// </summary>
		public string About { get; set; }

		/// <summary>
		/// Url path of this page relative to the wiki root
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// The path from the root of the repository
		/// </summary>
		public string DocId { get; set; }

		/// <summary>
		/// The content of the file.
		/// </summary>
		public string Content { get; set; }

		public virtual List<Tag> Tags { get; set; }
		public virtual List<Category> Categories { get; set; }
	}
}