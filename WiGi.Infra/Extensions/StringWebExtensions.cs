namespace WiGi.Extensions
{
	public static class StringWebExtensions
	{
		/// <summary>
		/// Encode an Url string
		/// </summary>
		public static string UrlEncode(this string url)
		{
			return System.Web.HttpUtility.UrlEncode(url);
		}

		/// <summary>
		/// Converts a string that has been encoded for transmission in a URL into a
		/// decoded string.
		/// </summary>
		public static string UrlDecode(this string url)
		{
			return System.Web.HttpUtility.UrlDecode(url);
		}
	}
}
