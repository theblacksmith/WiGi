namespace WiGi
{
	/// <summary>
	/// General helper functions used inside WiGi
	/// </summary>
	public class WiGiDocs
	{
		/// <summary>
		/// Create the html for an &lt;a> tag linking to the official WiGi docs.
		/// </summary>
		/// <param name="docId"></param>
		/// <returns></returns>
		public static string ErrorLink(string docId)
		{
			return @"<a href=" + ErrorUrl(docId) + ">" + DocIdToTitle(docId) + "</a>";
		}

		/// <summary>
		/// Returns a url to the official WiGi docs.
		/// </summary>
		/// <param name="docId"></param>
		/// <returns></returns>
		public static string ErrorUrl(string docId)
		{
			return "https://github.com/theblacksmith/WiGi/wiki/" + docId;
		}

		private static string DocIdToTitle(string docId)
		{
			var parts = docId.Split('\\');
			var last = parts[parts.Length - 1];

			return last.Replace('-', ' ');
		}
	}
}
