namespace WiGi.Extensions
{
	using System.Diagnostics.Contracts;
	using System.Collections.Generic;
	using System.Linq;

	public static class CollectionsExtensions
	{
		public static Stack<T> Clone<T>(this Stack<T> stack)
		{
			Contract.Requires(stack != null);
			return new Stack<T>(stack.Reverse());
		}
	}
}
