namespace WiGi.Wiki
{
	using System.Globalization;
	using Extensions;

	/// <summary>
	/// Represents a Document Id which is used to reference pages or directories
	/// </summary>
	public class DocId
	{
		private string _value;

		public readonly string Value;

		public DocId(string docId)
		{
			Value = docId;
		}

		/// <summary>
		/// Always returns true (for now)
		/// </summary>
		/// <param name="docId">The document id to validate</param>
		/// <returns></returns>
		public static bool IsValid(string docId)
		{
			return true;
		}

		/// <summary>
		/// Returns a DocId instance for the path provided
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static DocId ForPath(string path)
		{
			return new DocId(path.Replace("\\", "/").LeftOf('.'));
		}

		/// <summary>
		/// Returns this doc id as a title cased string, Like This One.
		/// </summary>
		/// <returns></returns>
		public string ToTileCase()
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ToText());
		}

		/// <summary>
		/// Returns the doc id converted to text.
		/// </summary>
		/// <returns></returns>
		public string ToText()
		{
			return Filename().Replace("-", " ");
		}

		private string Filename()
		{
			return Value.RightOfLast('/').LeftOf('.');
		}

		// todo: Create overload that accepts html attributes
		/// <summary>
		/// Returns the html string of a link tag referencing this doc id
		/// </summary>
		/// <returns></returns>
		public string ToLink()
		{
			return string.Format("<a href=\"{0}\">{1}</a>", Value, ToText());
		}
	}
}
