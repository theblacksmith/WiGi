namespace WiGi.Wiki
{
	using System.Collections.Generic;

	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public virtual List<Page> Pages { get; set; }
	}
}