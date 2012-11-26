namespace WiGi.UI.Models
{
	using System.Collections.Generic;
	using AutoMapper;
	using Wiki;

	public class PageViewModel
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
		/// The original content of the page.
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// The parsed content of the page.
		/// </summary>
		public string Html { get; set; }

		public virtual List<Tag> Tags { get; set; }
		public virtual List<Category> Categories { get; set; }

		public static void Map()
		{
			Mapper.CreateMap<Page, PageViewModel>()
				.ForMember(pvm => pvm.About, a => a.MapFrom(p => p.About));

			Mapper.CreateMap<PageViewModel, Page>()
				.ForSourceMember(m => m.Html, c => c.Ignore());
		}
	}
}