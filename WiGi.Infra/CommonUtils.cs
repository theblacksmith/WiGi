namespace WiGi
{
	using System;

	public class CommonUtils
	{
		public static T To<T>(string text)
		{
			return (T)Convert.ChangeType(text, typeof(T));
		}
	}
}
