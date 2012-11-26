namespace WiGi.Exceptions
{
	using System;

	public class ParserNotFoundException : Exception
	{
		public ParserNotFoundException(string extension) : base("There is no parser registered to " + extension + " files")
		{
		}
	}
}
