namespace WiGi.Extensions
{
	using System;
	using System.Globalization;

	public static class StringExtensions
	{
		/// <summary>
		/// Determines whether the string is null, empty or contains only white spaces.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the specified input is null, empty or contains only white spaces; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrWhiteSpace(this string input)
		{
			return String.IsNullOrWhiteSpace(input);
		}

		/// <summary>
		/// Returns the substring from the first character to the first appearence of <param name="marker" />.
		/// The marker itself won't be returned.
		/// </summary>
		/// <param name="marker">The character which marks the end of the substring</param>
		/// <returns></returns>
		public static string LeftOf(this string input, char marker)
		{
			var pos = input.IndexOf(marker);

			if (pos < 0 || pos >= input.Length)
				return input;

			return input.Substring(0, pos);
		}

		/// <summary>
		/// Returns the substring from the first character to the last appearence of <param name="marker" />.
		/// The marker itself won't be returned.
		/// </summary>
		/// <param name="marker">The character which marks the end of the substring</param>
		/// <returns></returns>
		public static string LeftOfLast(this string input, char marker)
		{
			var pos = input.LastIndexOf(marker);

			if (pos < 0 || pos >= input.Length)
				return input;

			return input.Substring(0, pos);
		}

		/// <summary>
		/// Returns a substring starting after the first appearence of <param name="marker" />.
		/// The marker itself won't be returned.
		/// </summary>
		/// <param name="marker">The character which marks the beggining of the substring</param>
		/// <returns></returns>
		public static string RightOf(this string input, char marker)
		{
			var pos = input.IndexOf(marker) + 1;

			if (pos < 0 || pos >= input.Length)
				return "";

			return input.Substring(pos);
		}

		/// <summary>
		/// Returns a substring starting after the last appearence of <param name="marker" />.
		/// The marker itself won't be returned.
		/// </summary>
		/// <param name="marker">The character which marks the beggining of the substring</param>
		/// <returns></returns>
		public static string RightOfLast(this string input, char marker)
		{
			var pos = input.LastIndexOf(marker) + 1;

			if (pos < 0 || pos >= input.Length)
				return "";

			return input.Substring(pos);
		}

		/// <summary>
		/// Converts the a string to "Title Case"
		/// </summary>
		/// <returns>Title Cased String</returns>
		public static string ToTitleCase(this string input)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
		}

		/// <summary>
		/// Converts the first character in a string to upper case.
		/// </summary>
		public static string UpperCaseFirst(this string input)
		{
			return Char.ToUpper(input[0]) + input.Substring(1);
		}
	}
}
